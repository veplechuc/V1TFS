using System;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

using Microsoft.TeamFoundation.VersionControl.Common;
using Microsoft.TeamFoundation.Build.Client;
using VersionOne.ServerConnector.Entities;
using VersionOne.TFS2010.DataLayer;
using VersionOneTFS2010.DataLayer.Providers;
using VersionOneTFSServer.Interfaces;
using VersionOneTFSServer.ServiceErrors;
using Environment = System.Environment;

namespace VersionOneTFSServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    [ServiceErrorBehavior(typeof(ServiceErrorHandler))]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service : IService
    {
        private readonly Lazy<V1Component> v1Component = new Lazy<V1Component>(() => 
            {
                var component = new V1Component(Utils.GetV1Settings());
                var validationResult = component.ValidateConnection();

                if(!validationResult) 
                {
                    throw new InvalidOperationException("VersionOne connection could not be established.");
                }

                return component;
            });

        public void Notify(string eventXml, string tfsIdentityXml)
        {

            Debug.instance().WriteNotificationMessage(eventXml, tfsIdentityXml);

            var buildEvent = false;
            var checkinEvent = false;
            var buildIndex = eventXml.IndexOf("BuildCompletionEvent2", 0, 255);
            
            if (buildIndex >= 0) 
            {
                buildEvent = true;
            }

            var checkinIndex = eventXml.IndexOf("CheckinEvent", 0, 255);

            if(checkinIndex >= 0) 
            {
                checkinEvent = true;
            }

            if (checkinEvent && buildEvent)
            {
                if (buildIndex < checkinIndex)
                {
                    checkinEvent = false;
                }
                else
                {
                    buildEvent = false;
                }
            }

            
           if (checkinEvent)
                {
                    Debug.instance().Write("CheckIn Event");
                    var checkInxs = new XmlSerializer(typeof(CheckinEvent));
                    var sr = new StringReader(eventXml);
                    var checkInData = checkInxs.Deserialize(sr) as CheckinEvent;
                    
                    if (checkInData != null) 
                    {
                        OnCheckInEvent(checkInData);
                    }
                }

           if (buildEvent)
                {
                    Debug.instance().Write("Build Event");
                    var buildCompletionxs = new XmlSerializer(typeof(BuildCompletionEvent2));
                    var sr = new StringReader(eventXml);
                    var buildCompletionData = buildCompletionxs.Deserialize(sr) as BuildCompletionEvent2;
                    
                    if (buildCompletionData != null) 
                    {
                        OnBuildCompletionEvent(buildCompletionData);
                    }
                }
            
        }

        /// <summary>
        /// Handle check-in event. We always create a changeset on checkin, since builds may want to reference the changeset.
        /// </summary>
        private void OnCheckInEvent(CheckinEvent e)
        {
            Debug.instance().Write("Process CheckIn Event for " + e.Number + " from " + e.TeamProject);
            
            var primaryWorkItems = GetPrimaryWorkitemsInComment(e.Comment);

            var changeSet = v1Component.Value.CreateChangeSet(e.Owner + " " + e.CreationDate, "TFS:" + e.Number, e.Comment);

            foreach (var clientArtifact in e.Artifacts.Cast<ClientArtifact>().Where(x => x.Type == "Changeset")) 
            {
                v1Component.Value.CreateLink(new Link(clientArtifact.Url, clientArtifact.Type, true), changeSet);
            }

            changeSet.PrimaryWorkitems = primaryWorkItems.Select(ValueId.FromEntity).ToArray();
            v1Component.Value.Save(changeSet);
                
            Debug.instance().Write("Saved Changeset for " + e.Number);
        }

        private IEnumerable<PrimaryWorkitem> GetPrimaryWorkitemsInComment(string comment)
        {
            var regex = new Regex(new ConfigurationProvider().TfsWorkItemRegex);
            var numbers = regex.Matches(comment).Cast<Match>().Select(x => x.Value).ToList();
            return v1Component.Value.GetRelatedPrimaryWorkitems(numbers);
        }

        private void OnBuildCompletionEvent(BuildCompletionEvent2 e)
        {
            Debug.instance().Write("Process Build Number " + e.BuildNumber + " from " + e.TeamProject);

            var tfs = Utils.ConnectToTfs();
            var buildStore = (IBuildServer) tfs.GetService(typeof(IBuildServer));

            var url = new Uri(e.Url);
            var buildUri = HttpUtility.ParseQueryString(url.Query).Get("builduri");
            var detail = buildStore.GetBuild(new Uri(buildUri));
            detail.RefreshAllDetails();

            var changeSetsData = InformationNodeConverters.GetAssociatedChangesets(detail);

            Debug.instance().Write("Number of ChangeSets: " + changeSetsData.Count);

            var reference = e.DefinitionPath;
            var index = reference.LastIndexOf("\\");

            if(index >= 0) 
            {
                reference = reference.Substring(index + 1);
            }

            var results = v1Component.Value.GetBuildProjects(reference);

            if(results.Count == 0) 
            {
                Debug.instance().Write("No results for reference " + reference);
            }

            foreach (var buildProject in results)
            {
                var ts = DateTime.Parse(e.FinishTime) - DateTime.Parse(e.StartTime);
                var buildRun = v1Component.Value.CreateBuildRun(buildProject, e.BuildNumber, DateTime.Parse(e.FinishTime), ts.Seconds);
                
                var statuses = v1Component.Value.GetBuildRunStatuses();
                var status = e.StatusCode == "Succeeded"
                                 ? statuses.First(x => x.Name == "Passed")
                                 : statuses.First(x => x.Name == "Failed");
                buildRun.Status = status;
                
                v1Component.Value.CreateLink(new Link(e.Url, "TFS Build Results", true), buildRun);

                var descriptionBuilder = new StringBuilder();

                foreach (var csd in changeSetsData)
                {
                    var changeSets = v1Component.Value.GetChangeSets("TFS:" + csd.ChangesetId);
                    
                    foreach (var changeSet in changeSets)
                    {
                        if(descriptionBuilder.Length > 0) 
                        {
                            descriptionBuilder.Append(Environment.NewLine);
                        }

                        descriptionBuilder.Append(changeSet.Description);
                        
                        foreach (var primaryWorkitem in v1Component.Value.GetPrimaryWorkitems(changeSet)) 
                        {
                            var remove = v1Component.Value.GetBuildRuns(primaryWorkitem, buildProject);
                            
                            v1Component.Value.RemoveBuildRunsFromWorkitem(primaryWorkitem, remove);
                            v1Component.Value.Save(primaryWorkitem);
                            
                            v1Component.Value.AddBuildRunsToWorkitem(primaryWorkitem, new[]{buildRun});
                            v1Component.Value.Save(primaryWorkitem);
                        }
                    }

                    v1Component.Value.AddChangeSetsToBuildRun(buildRun, changeSets);
                    v1Component.Value.Save(buildRun);
                }

                buildRun.Description = descriptionBuilder.ToString();
                v1Component.Value.Save(buildRun);
            }

            Debug.instance().Write("BuildRun Save Successful");
        }
    }
}
