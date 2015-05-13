# ApkVersionUpdater
Simple tool to auto update apk manifest version based on assembly version.

Usage:
Just add it as post-compilation step to your project config file.

this is sample excerpt from one of my projects:

  <PropertyGroup>
    <PostBuildEvent>$(SolutionDir)\tools\VersionUpdater $(TargetPath) $(ProjectDir)\Properties\AndroidManifest.xml</PostBuildEvent>
  </PropertyGroup>
