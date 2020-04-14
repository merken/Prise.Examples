var target = Argument("target", "default");
var configuration = Argument("configuration", "Debug");
var net2projects = new[] { "ProductsAPIControllerPlugin", "TwitterWidgetPlugin", "WeatherWidgetPlugin" };
var net3projects = new[] { "OrdersAPIControllerPlugin" };

private void CleanProject(string projectDirectory){
    var projectFile = $"Plugins/{projectDirectory}/{projectDirectory}.csproj";
    var bin = $"Plugins/{projectDirectory}/bin";
    var obj = $"Plugins/{projectDirectory}/obj";

    var deleteSettings = new DeleteDirectorySettings{
        Force= true,
        Recursive = true
    };

    var cleanSettings = new DotNetCoreCleanSettings
    {
        Configuration = configuration
    };
    if (DirectoryExists(bin))
    {
      DeleteDirectory(bin, deleteSettings);
    }
    if (DirectoryExists(obj))
    {
      DeleteDirectory(obj, deleteSettings);
    }
    DotNetCoreClean(projectFile, cleanSettings);
}

Task("clean").Does( () =>
{ 
  var allProjects = net2projects.Union(net3projects);
  foreach (var project in allProjects)
  {
    CleanProject(project);
  }
});

Task("build")
  .IsDependentOn("clean")
  .Does( () =>
{ 
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration
    };

    var allProjects = net2projects.Union(net3projects);
    foreach (var project in allProjects)
    {
      DotNetCoreBuild($"Plugins/{project}/{project}.csproj", settings);
    }
});

Task("publish")
  .IsDependentOn("build")
  .Does(() =>
  { 
    var allProjects = net2projects.Union(net3projects);
    foreach (var project in allProjects)
    {
      DotNetCorePublish($"Plugins/{project}/{project}.csproj", new DotNetCorePublishSettings
      {
          NoBuild = true,
          Configuration = configuration,
          OutputDirectory = $"publish/{project}"
      });
    }
  });

Task("copy-to-apphost")
  .IsDependentOn("publish")
  .Does(() =>
  {
    foreach (var project in net2projects)
    {
      CopyDirectory($"publish/{project}", $"MyHost2/bin/debug/netcoreapp2.1/Plugins/{project}");
    }

    foreach (var project in net2projects.Union(net3projects))
    {
      CopyDirectory($"publish/{project}", $"MyHost/bin/debug/netcoreapp3.1/Plugins/{project}");
    }
  });

Task("default")
  .IsDependentOn("copy-to-apphost");

RunTarget(target);