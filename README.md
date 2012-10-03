# VersionOne Integration with Microsoft Team Foundation Server #
Copyright (c) 2008-2012 VersionOne, Inc.
All rights reserved.

VersionOne's integration with Microsoft Team Foundation Server (V1TFS) provides 
your organization with visibility into change while freeing the development 
team to focus on deliverable. This integration is comprised of two parts: The 
TFS Listener and the VersionOne Check-in Policy.

The TFS Listener is responsible for responding to check-in and build events 
from TFS. For each qualifying check-in event the listener creates a "ChangeSet" 
in VersionOne, providing the team visibility into the code changes for a story 
or defect. This visibility can be useful when tracking down defects or 
performing code reviews. For each qualifying build event, the listener creates 
a "BuildRun" in VersionOne. If possible BuildRuns are associated with stories 
and/or defect in VersionOne , providing the organization visibility into build 
contents. This visibility is useful when selecting a build to release, 
identifying problem builds, or generating release notes.

In order for the Listener to create associations in VersionOne, the check-in 
comment must include a VersionOne story or defect ID, such as "S-01454". The 
VersionOne Check-in Policy ensures that this requirement is met. When this 
policy is enabled for a TFS project, each comments is scanned for a VersionOne 
ID. If not found, the Policy fails and prompts the user to select an ID from a 
dialog box. The user may select from items they own in active iterations, or 
from all items in active iterations.

This product includes software developed at VersionOne 
(http://versionone.com/). This product is open source and is licensed under a 
modified BSD license, which reflects our intent that software built with a 
dependency on V1TFS can be commercial or open source, as the authors see fit.

## System Requirements ##

### VersionOne ###
12.2 or above, including Team Edition

### Team Foundation Server ###
Microsoft Team Foundation Server 2010, 2012

### Clients ###
Visual Studio 2010, 2012
