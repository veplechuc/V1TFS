#!/bin/sh

set -xe

#obtain global api key value
MYGET_API_KEY="$2"

Configuration="$1"
PKGDIR="chocolateyPackage"
NUSPEC="VersionOne.TFS.PolicyInstaller.nuspec"
BUILT_VSIX="bin/$Configuration/VersionOneTFSPolicyInstaller.vsix"
MY_SOURCE="https://www.myget.org/F/versionone"

cp "$WORKSPACE/VersionOneTFSPolicyInstaller/$BUILT_VSIX" "$WORKSPACE/VersionOneTFSPolicyInstaller/$PKGDIR"

pushd "$WORKSPACE/VersionOneTFSPolicyInstaller/$PKGDIR"

cat > "$NUSPEC" <<EOF
<?xml version="1.0"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
  <metadata>
    <id>VersionOne.TFS.PolicyInstaller</id>
    <title>VersionOne.TFS.PolicyInstaller</title>
    <version>1.0</version>
    <authors>$ORGANIZATION_NAME</authors>
    <owners>$ORGANIZATION_NAME</owners>
    <summary>Ensure that commits checked into TFS via Visual Studio have VersionOne annotation.</summary>
    <description>VersionOne's TFS integration allows you to employ VersionOne in your TFS workflow.</description>
    <projectUrl>$GITHUB_WEB_URL</projectUrl>
    <tags>VersionOne TFS Visual Studio VS2012 VS2013</tags>
    <licenseUrl>$GITHUB_WEB_URL/blob/master/LICENSE.md</licenseUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
  </metadata>
  <files>
    <file src="tools\**" target="tools" />
  </files>
</package>
EOF


../../.nuget/nuget.exe pack "$NUSPEC"   # output ./Whatever.Nupkg?????
# NuGet SetApiKey <your key here> -source http://chocolatey.org/
# $MYGET_APIKEY
for NUPKG in VersionOne.TFS*.nupkg
do
    ../../.nuget/nuget.exe setApiKey "$MYGET_API_KEY" -Source "$MY_SOURCE"
    ../../.nuget/nuget.exe push "$NUPKG" -Source "$MY_SOURCE"
done