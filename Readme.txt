-----------------------------------------------------------------------------------
About the GTP Revit Toolkit

This readme.txt will tell you how to build and release this software.

Code Kill, 2023
https://github.com/srives/GTPRevitToolkit
-----------------------------------------------------------------------------------


1. GTP Toolkit is a program for use in Revit

2. This plugin was developed for use with GTP/STRATUS products

3. Assuming you have the ZIP containing all the source code for the GTP Toolkit:
   Place the GTP toolkit in some code path.  In the case of GTP, the code 
   is located here:
   
         C:\repos\GTP\GTPRevitToolkit
                                                                          
                09/03/2023  01:59 PM    <DIR>          bin
                09/03/2023  01:51 PM    <DIR>          Commands
                09/03/2023  01:51 PM    <DIR>          Core
                09/02/2023  02:32 PM    <DIR>          ExternalLibraries
                09/03/2023  01:51 PM    <DIR>          Filters
                09/02/2023  05:06 PM    <DIR>          install
                09/03/2023  01:59 PM    <DIR>          obj
                09/02/2023  02:32 PM    <DIR>          packages
                09/03/2023  01:51 PM    <DIR>          Properties
                09/03/2023  01:51 PM    <DIR>          Resources
                09/03/2023  01:51 PM    <DIR>          UI
                11/01/2022  06:33 PM               568 .editorconfig
                11/01/2022  06:33 PM             2,518 .gitattributes
                11/01/2022  06:33 PM             5,778 .gitignore
                11/09/2022  07:18 PM               710 app.config
                09/02/2023  05:01 PM            23,835 Application.cs
                09/03/2023  01:53 PM            45,777 GTPRevitToolkit.csproj
                12/03/2022  10:24 PM               617 GTPRevitToolkit.csproj.user
                09/03/2023  01:52 PM            13,651 GTPRevitToolkit.sln
                11/09/2022  07:18 PM             1,536 packages.config
                09/02/2023  11:08 PM             4,164 Readme.txt
                09/03/2023  01:58 PM               691 ReleaseNotes.txt

4.  Inside the revit-toolkit\install directory, you will find the build scripts.

5.  Directory of C:\repos\GTP\GTPRevitToolkit\install

                09/03/2023  02:01 PM             5,054 build.cmd
                09/03/2023  02:02 PM            14,267 CreateInstall.cmd
                11/21/2022  02:45 PM               201 excludeFiles.txt
                09/03/2023  02:00 PM             8,834 install.bat
                09/02/2023  05:58 PM                83 Install.message.txt
                09/03/2023  02:06 PM                56 InstallTitle.txt
                09/03/2023  02:03 PM             2,524 nogtp.cmd
                09/03/2023  02:22 PM             3,528 sign.cmd
                09/03/2023  02:05 PM               851 zipSource.cmd				
				
6. Optional: If you want to sign your DLLs and EXE files, edit sign.cmd with the path to your certificates and a password file

7. Open a command window and 

	cd C:\repos\GTP\GTPRevitToolkit\install\
	
   (or wherever your sourse path is: cd <yourpath>\install

8. Run CreateInstall.cmd
	
   a. First Install Visual Studio on your machine (the build.cmd script supports vs 2022, Professional 2017, or Community 2019).
      Whatever version you have, you can edit build.cmd with the location of your MSBUILD.exe program.
   b. CreateInstall.cmd will build the software and create the self-extracting EXE.
      Run CreateInstall.cmd twice. Once to create the 32bit installer, and once for 64bit: 
		CreateInstall -32
		CreateInstall -64   		
   c. The resulting EXEs are two installers:      
	    GTPRevitToolkit32bit.exe
	    GTPRevitToolkit64bit.exe	  
	  These are the installers to run on the target machines.	  
   d. When these installers run, they install the GTP toolkit to:   
        C:\Program Files (x86)\GTP Software, Inc\

9. This software uses a licesned version of WinZip self extracting EXE creator.

   http://winzip.com/en/product/self-extractor/

10. Developer notes:
    a. If you are a programmer, you can build the software by running:
          build.cmd 
	b. You can then have all the Revit manifest files point to your build by running
	      install -dev		
       Run this from the same directory that contains install.bat
       This won't copy DLLs, it will just point the revit adding to the x64 DLLs       
	c. You can clean out your binaries and erase the manifests by running:
	      nogtpp.cmd
	   This is useful if you are trying to isolate any copy or build problems.
	   	   
11.	Adding new versions of Revit
    At the time of this writing, Revit 2023 was the latest version of Revit.
	a. If you want to add Revit 24, update all the .CMD files
	b. Edit the .sln file for the project, and edit the .csproj
	c. Install Revit 2024, and copy the DLLs to:	
	
           C:\repos\GTP\GTPRevitToolkit\ExternalLibraries\2024\
		   
    ExternalLibraries is a directory that contains all the needed Revit DLLs
	