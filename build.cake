#tool "nuget:https://www.nuget.org/api/v2?package=GitVersion.CommandLine&version=3.5.4"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// DEFINE RUN CONSTANTS
//////////////////////////////////////////////////////////////////////

var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var PACKAGE_DIR = PROJECT_DIR + "package/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";
var SOLUTION = PROJECT_DIR + "NUnit.System.Linq.sln";

//////////////////////////////////////////////////////////////////////
// SET PACKAGE VERSION
//////////////////////////////////////////////////////////////////////

var version = "0.0.0";
var dbgSuffix = configuration == "Debug" ? "-dbg" : "";
var packageVersion = version + dbgSuffix;

//////////////////////////////////////////////////////////////////////
// CLEAN
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(BIN_DIR);
    });

//////////////////////////////////////////////////////////////////////
// RESTORE PACKAGES
//////////////////////////////////////////////////////////////////////

Task("NuGet-Package-Restore")
    .Does(() =>
    {
        NuGetRestore(SOLUTION);
    });

//////////////////////////////////////////////////////////////////////
// INITIALIZE BUILD
//////////////////////////////////////////////////////////////////////

Setup(context =>
{
    if (IsRunningOnWindows())
    {
        var settings = new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,
        };

        /*if (!BuildSystem.IsLocalBuild)
        {
            settings.LogFilePath = "console";
            settings.OutputType = GitVersionOutput.BuildServer;
        }*/

        var gitVersion = GitVersion(settings);

        Information("AssemblySemVer: " + gitVersion.AssemblySemVer);
        Information("BranchName: " + gitVersion.BranchName);
        Information("BuildMetaData: " + gitVersion.BuildMetaData);
        Information("BuildMetaDataPadded: " + gitVersion.BuildMetaDataPadded);
        Information("CommitDate: " + gitVersion.CommitDate);
        Information("CommitsSinceVersionSource: " + gitVersion.CommitsSinceVersionSource);
        Information("CommitsSinceVersionSourcePadded: " + gitVersion.CommitsSinceVersionSourcePadded);
        Information("FullBuildMetaData: " + gitVersion.FullBuildMetaData);
        Information("FullSemVer: " + gitVersion.FullSemVer);
        Information("InformationalVersion: " + gitVersion.InformationalVersion);
        Information("LegacySemVer: " + gitVersion.LegacySemVer);
        Information("LegacySemVerPadded: " + gitVersion.LegacySemVerPadded);
        Information("Major: " + gitVersion.Major);
        Information("Minor: " + gitVersion.Minor);
        Information("NuGetVersion: " + gitVersion.NuGetVersion);
        Information("NuGetVersionV2: " + gitVersion.NuGetVersionV2);
        Information("Patch: " + gitVersion.Patch);
        Information("PreReleaseLabel: " + gitVersion.PreReleaseLabel);
        Information("PreReleaseNumber: " + gitVersion.PreReleaseNumber);
        Information("PreReleaseTag: " + gitVersion.PreReleaseTag);
        Information("PreReleaseTagWithDash: " + gitVersion.PreReleaseTagWithDash);
        Information("SemVer: " + gitVersion.SemVer);
        Information("Sha: " + gitVersion.Sha);

        packageVersion = gitVersion.NuGetVersion ?? context.EnvironmentVariable("GitVersion_NuGetVersion");
    }

    if (string.IsNullOrWhiteSpace(packageVersion))
    {
        Warning("The package version is null or empty.");
    }
});

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("NuGet-Package-Restore")
    .Does(() =>
    {
        if (IsRunningOnWindows())
        {
            // Use MSBuild
            MSBuild(SOLUTION, new MSBuildSettings()
                .SetConfiguration(configuration)
                .SetMSBuildPlatform(MSBuildPlatform.Automatic)
                .SetVerbosity(Verbosity.Minimal)
                .SetNodeReuse(false));
        }
        else
        {
            // Use XBuild
            XBuild(SOLUTION, new XBuildSettings()
                .WithTarget("Build")
                .WithProperty("Configuration", configuration)
                .SetVerbosity(Verbosity.Minimal));
        }
    });

//////////////////////////////////////////////////////////////////////
// PACKAGE
//////////////////////////////////////////////////////////////////////

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CreateDirectory(PACKAGE_DIR);

        NuGetPack("nuget/NUnit.System.Linq.nuspec", new NuGetPackSettings()
        {
            Version = packageVersion,
            BasePath = BIN_DIR,
            OutputDirectory = PACKAGE_DIR
        });
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Appveyor")
    .IsDependentOn("Build")
    .IsDependentOn("Package");

Task("Travis")
    .IsDependentOn("Build");

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);