using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Integrations.Core.Interfaces;
using VersionOne.SDK.APIClient;
using VersionOne.ServerConnector;
using VersionOne.ServerConnector.Entities;
using VersionOne.ServerConnector.Filters;
using VersionOne.ServiceHost.Core.Configuration;
using VersionOne.ServiceHost.Core.Logging;
using VersionOneProcessorSettings = VersionOne.ServiceHost.Core.Configuration.VersionOneSettings;

namespace VersionOne.TFS2010.DataLayer 
{
    // TODO enable logging
    public class V1Component 
    {
        public readonly VersionOneSettings Settings;
        
        private readonly VersionOneProcessor processor;

        public const string ParentNumberProperty = "Parent.Number";

        public V1Component(VersionOneSettings settings) 
        {
            Settings = settings;
            var nativeSettings = ConvertSettings(settings);
            processor = new VersionOneProcessor(nativeSettings, new BlackholeLogger());
            AddProperties();
        }

        private void AddProperties() 
        {
            processor.AddProperty(Workitem.NumberProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            processor.AddProperty(Entity.NameProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            processor.AddProperty(Entity.DescriptionProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            processor.AddProperty(PrimaryWorkitem.CompletedInBuildRunsProperty, VersionOneProcessor.PrimaryWorkitemType, false);
            processor.AddProperty(Workitem.OwnersProperty, VersionOneProcessor.PrimaryWorkitemType, false);

            processor.AddProperty(Entity.NameProperty, VersionOneProcessor.BuildProjectType, false);
            
            processor.AddProperty(Workitem.NumberProperty, VersionOneProcessor.WorkitemType, false);
            processor.AddProperty(Entity.NameProperty, VersionOneProcessor.WorkitemType, false);
            processor.AddProperty(Workitem.OwnersProperty, VersionOneProcessor.WorkitemType, false);
            processor.AddProperty(ParentNumberProperty, VersionOneProcessor.WorkitemType, false);

            processor.AddProperty(Entity.NameProperty, VersionOneProcessor.ChangeSetType, false);
            processor.AddProperty(Entity.DescriptionProperty, VersionOneProcessor.ChangeSetType, false);
            processor.AddProperty(BaseEntity.ReferenceProperty, VersionOneProcessor.ChangeSetType, false);
            processor.AddProperty(ChangeSet.PrimaryWorkitemsProperty, VersionOneProcessor.ChangeSetType, false);

            processor.AddProperty(Entity.StatusProperty, VersionOneProcessor.BuildRunType, true);
            processor.AddProperty(BuildRun.ElapsedProperty, VersionOneProcessor.BuildRunType, false);
            processor.AddProperty(Entity.NameProperty, VersionOneProcessor.BuildRunType, false);
            processor.AddProperty(BuildRun.DateProperty, VersionOneProcessor.BuildRunType, false);
            processor.AddProperty(BuildRun.BuildProjectProperty, VersionOneProcessor.BuildRunType, false);
            processor.AddProperty(BuildRun.ChangeSetsProperty, VersionOneProcessor.BuildRunType, false);

            processor.AddProperty(Member.UsernameProperty, VersionOneProcessor.MemberType, false);
        }

        public bool ValidateConnection() 
        {
            return processor.ValidateConnection();
        }

        public ICollection<PrimaryWorkitem> GetRelatedPrimaryWorkitems(ICollection<string> numbers) 
        {
            if(numbers == null || numbers.Count == 0) 
            {
                return new List<PrimaryWorkitem>();
            }

            var filters = numbers.Select(number => Filter.Equal(Workitem.NumberProperty, number)).ToList();
            var items = processor.GetPrimaryWorkitems(GroupFilter.Or(filters.ToArray()));

            var unusedNumbers = numbers.Where(n => !items.Any(x => string.Equals(x.Number, n))).ToList();

            if(unusedNumbers.Count > 0) 
            {
                filters = unusedNumbers.Select(number => Filter.Equal("Children.Number", number)).ToList();
                var referencedViaChildren = processor.GetPrimaryWorkitems(GroupFilter.Or(filters.ToArray()));
                referencedViaChildren.Where(x => !items.Any(y => string.Equals(x.Number, y.Number))).ToList().ForEach(items.Add);
            }

            return items.Cast<PrimaryWorkitem>().ToList();
        }

        public ICollection<BuildProject> GetBuildProjects(string reference) 
        {
            var filter = Filter.Equal(BaseEntity.ReferenceProperty, reference);
            return processor.GetBuildProjects(filter);
        } 

        public ICollection<ChangeSet> GetChangeSets(string reference) 
        {
            var filter = Filter.Equal(BaseEntity.ReferenceProperty, reference);
            return processor.GetChangeSets(filter);
        } 

        public ChangeSet CreateChangeSet(string name, string reference, string description) 
        {
            return processor.CreateChangeSet(name, reference, description);
        }

        public BuildRun CreateBuildRun(BuildProject buildProject, string name, DateTime date, double elapsed) 
        {
            return processor.CreateBuildRun(buildProject, name, date, elapsed);
        }

        public void CreateLink(Link link, BaseEntity entity) 
        {
            processor.AddLinkToEntity(entity, link);
        }

        public void Save(BaseEntity entity) 
        {
            processor.Save(entity);
        }

        public ICollection<ValueId> GetBuildRunStatuses() 
        {
            return processor.GetBuildRunStatuses();
        } 

        public void AddChangeSetsToBuildRun(BuildRun buildRun, IEnumerable<ChangeSet> changeSets) 
        {
            var changeSetIds = new List<ValueId>(buildRun.ChangeSets);
            changeSetIds.AddRange(changeSets.Select(ValueId.FromEntity));
            buildRun.ChangeSets = changeSetIds.ToArray();
        }

        public ICollection<PrimaryWorkitem> GetPrimaryWorkitems(ChangeSet changeSet) 
        {
            return processor.GetPrimaryWorkitems(Filter.Equal("ChangeSets", changeSet.Id)).Cast<PrimaryWorkitem>().ToList();
        }

        public ICollection<BuildRun> GetBuildRuns(PrimaryWorkitem primaryWorkitem, BuildProject buildProject) 
        {
            return processor.GetBuildRuns(GroupFilter.And(
                Filter.Equal("CompletesPrimaryWorkitems", primaryWorkitem.Id),
                Filter.Equal("BuildProject", buildProject.Id)));
        }

        public void RemoveBuildRunsFromWorkitem(PrimaryWorkitem workitem, IEnumerable<BuildRun> buildRuns) 
        {
            var remove = (buildRuns ?? new List<BuildRun>()).Select(ValueId.FromEntity).ToList();
            var buildRunIds = workitem.CompletedInBuildRuns.Where(x => !remove.Any(y => y.Equals(x))).ToArray();
            workitem.CompletedInBuildRuns = buildRunIds;
        }

        public void AddBuildRunsToWorkitem(PrimaryWorkitem workitem, IEnumerable<BuildRun> buildRuns) 
        {
            var buildRunIds = new List<ValueId>(workitem.CompletedInBuildRuns);
            buildRunIds.AddRange(buildRuns.Select(ValueId.FromEntity));
            workitem.CompletedInBuildRuns = buildRunIds.ToArray();
        }

        public string GetLoggedInMemberUsername() 
        {
            var member = processor.GetLoggedInMember();
            return member.Username;
        }

        public ICollection<PrimaryWorkitem> GetActivePrimaryWorkitems() 
        {
            return processor.GetPrimaryWorkitems(
                GroupFilter.And(
                    Filter.Equal("AssetState", AssetState.Active),
                    Filter.Equal("Timebox.State.Code", "ACTV")
                ), SortBy.Ascending("Order")).Cast<PrimaryWorkitem>().ToList();
        }

        public ICollection<Workitem> GetActiveSecondaryWorkitems() 
        {
            return processor.GetWorkitems(
                VersionOneProcessor.WorkitemType,
                GroupFilter.And(
                    Filter.OfTypes(VersionOneProcessor.TaskType, VersionOneProcessor.TestType),
                    Filter.Equal("Parent.AssetType", VersionOneProcessor.PrimaryWorkitemType),
                    Filter.Equal("Parent.AssetState", AssetState.Active),
                    Filter.Equal("AssetState", AssetState.Active),
                    Filter.Equal("Parent.Timebox.State.Code", "ACTV")));
        } 

        public static VersionOneProcessorSettings ConvertSettings(IVersionOneSettings settings) 
        {
            return new VersionOneProcessorSettings 
            {
                Url = settings.Path,
                Username = settings.Username,
                Password = settings.Password,
                IntegratedAuth = settings.Integrated,
                ProxySettings = settings.ProxySettings == null || !settings.ProxySettings.ProxyIsEnabled
                                    ? null
                                    : new ProxySettings 
                                    {
                                        Url = settings.ProxySettings.Url.ToString(),
                                        Domain = settings.ProxySettings.Domain,
                                        Username = settings.ProxySettings.Username,
                                        Password = settings.ProxySettings.Password,
                                        Enabled = settings.ProxySettings.ProxyIsEnabled,
                                    }
            };
        }

        private class BlackholeLogger : ILogger 
        {
            public void Log(string message) { }
            public void Log(string message, Exception exception) { }
            public void Log(LogMessage.SeverityType severity, string message) { }
            public void Log(LogMessage.SeverityType severity, string message, Exception exception) { }
	        public void LogVersionOneConfiguration(LogMessage.SeverityType severity, XmlElement config) { }
	        public void LogVersionOneConnectionInformation(LogMessage.SeverityType severity, string metaVersion, string memberOid, string defaultMemberRole) { }
        }
    }
}
