using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VersionOne.ServerConnector.Entities;
using VersionOne.TFS2010.DataLayer;

namespace VersionOne.TFS2010.Tests {
    /// <summary>
    /// These tests rely upon existing data and are currently used only as aid to porting ObjectModel code.
    /// In future, it is nice to make them create/wipe out data and perform required checks instead of manual lookup.
    /// </summary>
    [TestFixture]
    public class V1ComponentTester {
        private V1Component component;
        
        [TestFixtureSetUp]
        public void TestFixtureSetUp() {
            var settings = new VersionOneSettings {
                Path = "http://integsrv01/VersionOne12/",
                Username = "stanf",
                Password = "exigen",
                ProxySettings = null
            };

            component = new V1Component(settings);
            component.ValidateConnection();
        }

        [Test]
        public void RetrievePrimaryWorkitems() {
            var workitems = component.GetRelatedPrimaryWorkitems(new List<string> {"B-01003", "B-01004", "TK-01007", "TK-01009", "NA-01001"});
            Assert.IsTrue(workitems.Count == 3);
            Assert.IsTrue(workitems.Any(x => x.Number == "B-01003"));
            Assert.IsTrue(workitems.Any(x => x.Number == "B-01004"));
            Assert.IsTrue(workitems.Any(x => x.Number == "B-01007"));
            Assert.IsFalse(workitems.Any(x => x.Number == "TK-01007"));
            Assert.IsFalse(workitems.Any(x => x.Number == "TK-01009"));
            Assert.IsFalse(workitems.Any(x => x.Number == "NA-01001"));
        }

        [Test]
        public void RetrievePrimaryWorkitemsNoSecondaryNumbers() {
            var workitems = component.GetRelatedPrimaryWorkitems(new List<string> {"B-01003", "B-01004"});
            Assert.IsTrue(workitems.Count == 2);
            Assert.IsTrue(workitems.Any(x => x.Number == "B-01003"));
            Assert.IsTrue(workitems.Any(x => x.Number == "B-01004"));
        }

        [Test]
        public void GetBuildProjects() {
            var buildProjects = component.GetBuildProjects("TfsProject");
            Assert.IsTrue(buildProjects.Count > 0);
        }

        [Test]
        public void GetChangeSets() {
            var changeSets = component.GetChangeSets("TFS:10000");
            Assert.IsTrue(changeSets.Count > 0);
            Assert.AreEqual("Test changeset", changeSets.First().Name);
        }

        [Test]
        public void CreateChangeSet() {
            var changeSet = component.CreateChangeSet("Test changeset", "TFS:10000", "Created from integration tests");
            Assert.IsTrue(changeSet.Name == "Test changeset");
        }

        [Test]
        public void CreateBuildRun() {
            var buildProject = component.GetBuildProjects("TfsProject").First();
            var buildRun = component.CreateBuildRun(buildProject, "Test build run", DateTime.Now, 10.38);
            Assert.IsTrue(buildRun.Name == "Test build run");
            buildRun.Description = "We can also provide details";
            component.Save(buildRun);
        }

        [Test]
        public void AddLinkToChangeSet() {
            var changeSet = component.CreateChangeSet("Test changeset with link", "TFS:10001", "Created from integration tests");
            component.CreateLink(new Link("http://google.com", "ChangeSet link", true), changeSet);
        }

        [Test]
        public void AddLinkToBuildRun() {
            var buildProject = component.GetBuildProjects("TfsProject").First();
            var buildRun = component.CreateBuildRun(buildProject, "Test build run with link", DateTime.Now, 4.2);
            component.CreateLink(new Link("http://google.com", "BuildRun link", true), buildRun);
        }

        [Test]
        public void AddPrimaryWorkitemReferencesToChangeSet() {
            var workitems = component.GetRelatedPrimaryWorkitems(new List<string> {"TK-01009"});
            var changeSet = component.CreateChangeSet("Test changeset with referenced workitems", "TFS:10000", "Created from integration tests");
            changeSet.PrimaryWorkitems = workitems.Select(ValueId.FromEntity).ToArray();
            component.Save(changeSet);
        }

        [Test]
        public void SetBuildRunStatus() {
            var buildRunStatuses = component.GetBuildRunStatuses();
            var buildProject = component.GetBuildProjects("TfsProject").First();
            var buildRun = component.CreateBuildRun(buildProject, "Test build run", DateTime.Now, 8.1);
            buildRun.Status = buildRunStatuses.First();
            component.Save(buildRun);
        }

        [Test]
        public void AddChangesetsToBuildRun() {
            var buildProject = component.GetBuildProjects("TfsProject").First();
            var buildRun = component.CreateBuildRun(buildProject, "Build run with changesets", DateTime.Now, 1.9);
            var changeSet1 = component.CreateChangeSet("First test changeset", "TFS:10002", "Created from integration tests");
            var changeSet2 = component.CreateChangeSet("Second test changeset", "TFS:10002", "Created from integration tests");
            buildRun.ChangeSets = new[] {ValueId.FromEntity(changeSet1), ValueId.FromEntity(changeSet2)};
            component.Save(buildRun);
            buildRun.ChangeSets = new[] { ValueId.FromEntity(changeSet2) };
            component.Save(buildRun);
        }

        [Test]
        public void PrimaryWorkitemCompletedInBuildRuns() {
            var workitem = component.GetRelatedPrimaryWorkitems(new List<string> {"B-01003"}).First();
            var buildProject = component.GetBuildProjects("TfsProject").First();
            var buildRun1 = component.CreateBuildRun(buildProject, "Build run 1", DateTime.Now, 1.9);
            var buildRun2 = component.CreateBuildRun(buildProject, "Build run 2", DateTime.Now, 1.9);
            workitem.CompletedInBuildRuns = new[] {ValueId.FromEntity(buildRun1), ValueId.FromEntity(buildRun2)};
            component.Save(workitem);
        }

        [Test]
        public void GetPrimaryWorkitemsByChangeSet() {
            const string changeSetName = "Referenced from PW";

            var workitem = component.GetRelatedPrimaryWorkitems(new List<string> {"B-01003"}).First();
            var changeSet = component.CreateChangeSet(changeSetName, "TFS:10002", "Created from integration tests");
            changeSet.PrimaryWorkitems = new[] { ValueId.FromEntity(workitem)};
            component.Save(changeSet);

            var workitems = component.GetPrimaryWorkitems(changeSet);
            Assert.IsTrue(workitems.Count == 1);
            Assert.IsTrue(workitems.Any(x => x.Number == "B-01003"));
        }

        [Test]
        public void GetBuildRuns() {
            const string buildRunName = "Referencing Primary Workitem";

            var workitem = component.GetRelatedPrimaryWorkitems(new List<string> {"B-01028"}).First();

            var changeSet = component.CreateChangeSet("One", "TFS:10002", "Created from integration tests");
            changeSet.PrimaryWorkitems = new[] { ValueId.FromEntity(workitem)};
            component.Save(changeSet);

            var buildProject = component.GetBuildProjects("TfsProject").First();
            var buildRun = component.CreateBuildRun(buildProject, buildRunName, DateTime.Now, 1.9);
            buildRun.ChangeSets = new[] {ValueId.FromEntity(changeSet)};
            component.Save(buildRun);
            workitem.CompletedInBuildRuns = new[] {ValueId.FromEntity(buildRun)};
            component.Save(workitem);

            var buildRuns = component.GetBuildRuns(workitem, buildProject);
            Assert.IsTrue(buildRuns.Count == 1);
            Assert.IsTrue(buildRuns.Any(x => x.Name == buildRunName));
        }

        [Test]
        public void AddAndRemoveBuildRuns() {
            var workitem = component.GetRelatedPrimaryWorkitems(new List<string> {"B-01031"}).First();

            var buildProject = component.GetBuildProjects("TfsProject").First();
            var buildRun1 = component.CreateBuildRun(buildProject, "Build run to add/remove #1", DateTime.Now, 1.9);
            var buildRun2 = component.CreateBuildRun(buildProject, "Build run to add/remove #2", DateTime.Now, 1.9);
            var buildRun3 = component.CreateBuildRun(buildProject, "Build run to add/remove #3", DateTime.Now, 1.9);

            component.AddBuildRunsToWorkitem(workitem, new[] { buildRun1, buildRun2});
            component.Save(workitem);

            component.AddBuildRunsToWorkitem(workitem, new[] { buildRun3});
            component.Save(workitem);

            component.RemoveBuildRunsFromWorkitem(workitem, new[] { buildRun2 });
            component.Save(workitem);
        }
    }
}