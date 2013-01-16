@ECHO OFF
SETLOCAL
SET PROJECT="V1TFS2010.sln"
FOR /D %%D IN (%SYSTEMROOT%\Microsoft.NET\Framework\*) DO SET MSBUILD="%%D\MSBuild.exe"
REM SET MSBUILD=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
SET NUGET="%CD%\.nuget\NuGet.exe"
SET NUGET_SRC="http://packages.nuget.org/api/v2/;http://www.myget.org/F/versionone/"
SET BUILD_TOOLS=%1

ECHO.
ECHO Beginning build for %PROJECT%
ECHO Using msbuild at %MSBUILD%
ECHO Using nuget at %NUGET%
ECHO Using nuget sources %NUGET_SRC%
ECHO Using V1BuildTools at %BUILD_TOOLS%
ECHO Using signing key at %SIGNING_KEY%
ECHO.

%MSBUILD% %PROJECT% /m /t:Clean

REM FOR %%P IN (VersionOne.TFS2010.DataLayer\VersionOne.TFS2010.DataLayer.csproj VersionOne.TFS2010.Tests\VersionOne.TFS2010.Tests.csproj VersionOneTFSPolicy\VersionOneTFSPolicy.csproj VersionOneTFSServer\VersionOneTFSServer.csproj VersionOneTFSServerConfig\VersionOneTFSServerConfig.csproj) DO (
REM %MSBUILD% %%P /t:RestorePackages ^
REM /p:NuGetExePath=%NUGET% ^
REM /p:PackageSources=%NUGET_SRC% ^
REM /p:RequireRestoreConsent=false
REM )

REM FOR /D %%D IN (%CD%\*) DO (
REM ECHO %NUGET% update %%D\packages.config -Verbose -Source %NUGET_SRC% -repositoryPath %%D\packages
REM )

%MSBUILD% %PROJECT% ^
/p:V1BuildToolsPath=%BUILD_TOOLS% ^
/p:NuGetExePath=%NUGET% ^
/p:PackageSources=%NUGET_SRC% ^
/p:RequireRestoreConsent=false ^
/p:Configuration=Release ^
/p:Platform="x64" ^
/p:Major=2 ^
/p:Minor=0 ^
/p:Revision=1 ^
/p:AssemblyInformationalVersion="See https://github.com/versionone/V1TFS/wiki" ^
/p:AssemblyCopyright="Copyright 2008-2013, VersionOne, Inc. Licensed under modified BSD." ^
/p:CompanyName="VersionOne, Inc" ^
/p:AssemblyProduct="V1TFS" ^
/p:AssemblyTitle="VersionOne Team Foundation Server Integration" ^
/p:AssemblyDescription="VersionOne Team Foundation Server Integration Release Build" ^
/p:SignAssembly=%SIGN_ASSEMBLY% ^
/p:AssemblyOriginatorKeyFile=%SIGNING_KEY%

ENDLOCAL