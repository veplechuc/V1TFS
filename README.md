# VersionOne Integration with Microsoft Team Foundation Server #

V1TFS links VersionOne Workitems to TFS Check-ins and Builds. The link from Workitems to Check-ins makes it easier to track down the source of a defect and perform code reviews. The link from Workitems to Builds enables teams to measure progress in terms of working software and to identify problems sooner. Using the links from Workitems to Builds, the VersionOne BuildRun Report can help a Release Manager select an appropriate build for release and can be the starting point for release notes.

V1TFS has 2 components: The TFS Listener and the VersionOne Check-in Policy.

The TFS Listener is responsible for responding to check-in and build events from TFS. For each qualifying check-in event the listener creates a "ChangeSet" in VersionOne, providing the team visibility into the code changes for a story or defect. This visibility can be useful when tracking down defects or performing code reviews. For each qualifying build event, the listener creates a "BuildRun" in VersionOne. If possible, BuildRuns are associated with stories and/or defects in VersionOne providing the organization visibility into build contents. This visibility is useful when selecting a build to release, identifying problem builds, or generating release notes.

In order for the Listener to create associations in VersionOne, the check-in comment must include a VersionOne story or defect ID, such as "S-01454". The VersionOne Check-in Policy ensures that this requirement is met. When this policy is enabled for a TFS project, each comments is scanned for a VersionOne ID. If not found, the Policy fails and prompts the user to select an ID from a dialog box. The user may select from items they own in active iterations, or from all items in active iterations.

This product includes software developed at VersionOne (http://versionone.com/). This product is open source and is licensed under a modified BSD license, which reflects our intent that software built with a dependency on V1TFS can be commercial or open source, as the authors see fit.

![V1TFS Integration Sequence Diagram](https://raw.github.com/versionone/V1TFS/master/doc/images/V1TFS_Integration_Sequence.png "V1TFS Integration Sequence Diagram")

## System Requirements ##

### VersionOne ###
12.2 or above, including Team Edition

### Team Foundation Server ###
Microsoft Team Foundation Server 2010, 2012

### Clients ###
Visual Studio 2010, 2012 (not including Team Explorer)

## Install TFS Listener

The VersionOne TFS Listener is a WebService that must be installed on a machine that has access to your VersionOne Server and your Team Foundation Server. It is recommended that you install the Listener on the Team Foundation Server.

To install the TFS Listener, execute VersionOne.TFS.Listener.Installer.msi on the appropriate machine and follow the steps in the installation wizard. You can verify the listener by typing the following URL into your browser:
```url
http://[machine]:[port]/Service.svc
```

Replace [machine] with the name of the machine hosting the service [port] with the TCP port number that you provided in the installer.

For instance, if the machine hosting your TFS Server is called "TFS2010" and you specified port 9090 in the install, your URL would be:
```url
http://tfs2010:9090/Service.svc 
```
You should see a page similar to the following:
![Service Service](https://raw.github.com/versionone/V1TFS/master/doc/images/ListenerWorks.png "Listener Works")

## Configure the TFS Listener

In order for the TFS Listener to work it must know which VersionOne instance to use when creating assets and making associations and which TFS instance to use for listening to events. Configuring the TFS Listener is accomplished using the TFS Listener Configuration utility. This utility is installed as part of the TFS Listener installation. If you need to reconfigure the TFS Listener at a later date, this utility is available from the Start menu under Programs | VersionOne TFS Listener.

The utility has 3 tabs

### 1. VersionOne Server 

This tab allows you to to configure VersionOne connectivity. All of these parameters are required.
 
![VersionOne Server Tab](https://raw.github.com/versionone/V1TFS/master/doc/images/V1Config.jpg "VersionOne Server")

### 2. TFS Server 

This tab allows you to subscribe to TFS Events. All of these parameters are required
 
![TFS Server Tab](https://raw.github.com/versionone/V1TFS/master/doc/images/TFSConfig.jpg "TFS Server")

### 3. Advanced 

This tab allows you to configure the regular expression used to match VersionOne IDs in Check-in comments and enable a debug log. This configuration is optional since we set the default value to a working expression. We do not recommend you change this unless necessary.
 
![Advanced Tab](https://raw.github.com/versionone/V1TFS/master/doc/images/AdvancedConfig.jpg "Advanced")

## Install the VersionOne Check-in policy
The VersionOne Check-in Policy ensures that each TFS Check-in contains a VersionOne identifier. This policy must be installed on each machine running Visual Studio. To install the VersionOne Check-in policy, execute VersionOne.TFSPolicy.Installer.vsix on the client machine.

## Enable the VersionOne Check-in policy for a TFS Project

In order to be considered when performing a check-in, the VersionOne TFS Check-in policy must be enabled on a TFS project. Perform the following steps to enable the Check-in policy on a TFS Project.

### 1. Open Visual Studio
### 2. Open the Team Explorer
### 3. Right click on the desired project
### 4. Select "Team Project Settings"
### 5. Select "Source Control..."
![Team Project Settings -> Source Control...](https://raw.github.com/versionone/V1TFS/master/doc/images/TFS.EnablePolicy.1.jpg "Team Project Settings -> Source Control...")
### 6. Select the "Check-in Policy" tab
![Check-in Policy Tab](https://raw.github.com/versionone/V1TFS/master/doc/images/TFS.EnablePolicy.2.jpg "Check-in Policy")
### 7. Click "Add"
### 8. Select the "VersionOne Policy"
![VersionOne Policy](https://raw.github.com/versionone/V1TFS/master/doc/images/TFS.EnablePolicy.3.jpg "VersionOne Policy")
### 9. Click Ok to close the "Add Check-in Policy" dialog
### 10. Click Ok to close the "Source Control Settings" dialog

## Configure VersionOne Build Integration

In order to access TFS Build Runs in VersionOne you must configure the VersionOne application. The following steps describe how to enable build integration, create a build project, and assign that project to a project containing stories and defects.

If you are using Team Edition, you need to manually create the Build Project. Instructions for doing this are available on the VersionOne Community Site. 

### 1. Log into the VersionOne application as admin
### 2. Navigate to the Admin -> Configuration -> System page
### 3. Check the "Enable Build Integration" checkbox and click the Apply button
![Enable Build Integration](https://raw.github.com/versionone/V1TFS/master/doc/images/EnableBuildIntegration.jpg "Enable Build Integration")
### 4. Navigate to the Admin -> Projects -> Build Project page
![Build Projects](https://raw.github.com/versionone/V1TFS/master/doc/images/BuildProjects.jpg "Build Projects")
### 5. Click Add to add a new Build Project
### 6. Specify the following
1. Name: this is how the Build Project will be known to VersionOne users
2. Reference: this is the Build Project is known to TFS

### 7. Click Ok to save the new Build Project
### 8. Navigate to the Admin -> Projects -> Projects page
### 9. Click Edit on the row for the project you want associated with a Build Project
### 10. Using the "Build Projects" dropdown add the appropriate Build Project.
![Assign Build Project to Project](https://raw.github.com/versionone/V1TFS/master/doc/images/AssignBuildProjectToProject.jpg "Assign Build Project to Project")
### 11. Click Ok to accept the changes
### 12. Logout
