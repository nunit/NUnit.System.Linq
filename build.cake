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

var version = "0.6.0";
var modifier = "";
var dbgSuffix = configuration == "Debug" ? "-dbg" : "";
var packageVersion = version + modifier + dbgSuffix;

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
    GitVersion gitVersion;

    if (!BuildSystem.IsLocalBuild)
    {
        // TODO: Figure out if and how we can update the build number on Travis. @asbjornu
        if (BuildSystem.IsRunningOnAppVeyor)
        {
            var tag = AppVeyor.Environment.Repository.Tag;

            if (tag.IsTag)
            {
                packageVersion = tag.Name;
            }
            else
            {
                gitVersion = GitVersion(new GitVersionSettings
                {
                    UpdateAssemblyInfo = true,
                    LogFilePath = "console",
                    OutputType = GitVersionOutput.BuildServer
                });

                packageVersion = gitVersion.NuGetVersion;
            }

            AppVeyor.UpdateBuildVersion(packageVersion);
        }
    }
    else
    {
        gitVersion = GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json
        });

        packageVersion = gitVersion.NuGetVersion;
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