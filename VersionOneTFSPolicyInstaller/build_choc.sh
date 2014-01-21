#!/bin/sh

set -xe

PKGDIR="chocolateyPackage"
NUSPEC="VersionOne.TFS.PolicyInstaller.nuspec"
BUILT_VSIX="bin/$Configuration/VersionOneTFSPolicyInstaller.vsix"

cp "$BUILT_VSIX" "$PKGDIR"

pushd "$PKGDIR"

cat > "$NUSPEC" <<EOF
<?xml version="1.0"?>
<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
  <metadata>
    <id>VersionOne.TFS.Policy</id>
    <title>$PRODUCT_NAME</title>
    <version>$VERSION_NUMBER.$BUILD_NUMBER</version>
    <authors>$ORGANIZATION_NAME</authors>
    <owners>$ORGANIZATION_NAME</owners>
    <summary>Ensure that commits checked into TFS via Visual Studio have VersionOne annotation.</summary>
    <description>VersionOne's TFS integration allows you to employ VersionOne in your TFS workflow.</description>
    <projectUrl>$GITHUB_WEB_URL</projectUrl>
    <tags>VersionOne TFS Visual Studio VS2012 VS2013</tags>
    <licenseUrl>$GITHUB_WEB_URL/blob/master/LICENSE.md</licenseUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <dependencies>
    </dependencies>
    <releaseNotes>
    	`cat CHANGES.md`
    </releaseNotes>
  </metadata>
  <files>
    <file src="tools\**" target="tools" />
  </files>
</package>
EOF


cpack "$NUSPEC"   # output ./Whatever.Nupkg?????
# NuGet SetApiKey <your key here> -source http://chocolatey.org/
# $MYGET_APIKEY
cpush $NUPKG -Source $MYGET_URL 


