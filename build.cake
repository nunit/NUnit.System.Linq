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

var version = "0.4.0";
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

Task("InitializeBuild")
    .Does(() =>
    {
      if (BuildSystem.IsRunningOnAppVeyor)
      {
          var tag = AppVeyor.Environment.Repository.Tag;

          if (tag.IsTag)
          {
              packageVersion = tag.Name;
          }
          else
          {
              var buildNumber = AppVeyor.Environment.Build.Number;
              packageVersion = version + "-CI-" + buildNumber + dbgSuffix;
              if (AppVeyor.Environment.PullRequest.IsPullRequest)
                  packageVersion += "-PR-" + AppVeyor.Environment.PullRequest.Number;
              else if (AppVeyor.Environment.Repository.Branch.StartsWith("release", StringComparison.OrdinalIgnoreCase))
                  packageVersion += "-PRE-" + buildNumber;
              else
                  packageVersion += "-" + AppVeyor.Environment.Repository.Branch;
          }

          AppVeyor.UpdateBuildVersion(packageVersion);
      }
    });

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("InitializeBuild")
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