Where to Clone To
-------------------------------------------------
I am not positive it matters where you clone to,
but one of the batch files may be hard coded to

    C:\repos\GTP\GTPRevitToolkit


How to Build
-------------------------------------------------
Before you use the command line tools for the first time on a fresh GIT clone.
  First open in Visual Studio and build. For some reason, the build.cmd file has a hard time building fresh from the clone.

How to Upgrade to Newer Version of Revit
-------------------------------------------------
If you add a new version of Revit
Update all the .cmd and .bat files
Update the .csproj and .sln files
Copy the new DLLs from C:\Program Files\Autodesk\Revit 2025\ to the ExternalLibraries directory
              Copy AdWindows.dll, RevitAPI.dll, RevitAPIUI.dll


How to Debug
-------------------------------------------------
Run
   build.cmd
   install --dev
           Now you can attach from the debugger to Revit when the healthcheck is open