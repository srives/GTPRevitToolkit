@echo off
rem ----------------------------------------------------------------------------------------------
rem  For GTP Revit ToolKit
rem  Builds:  64bit (configurable to 32 bit)
rem  Note:    If you want to build ARM64, I don't think Revit supports that. 
rem
rem  Usage:
rem              build [-32 | -86 | -64] [-Release | -Debug] [YYYY]
rem
rem Examples:
rem              build 2023
rem
rem           Builds the Revit 2023 version of the GTP addin, 64bit mode
rem
rem              build 2023 -86
rem
rem           Builds Relase and Debug toolkit for Revit 2023 version of the GTP addin in 32bit mode
rem
rem              build 2019 -64 -Release
rem
rem           Builds Relase toolkit for Revit 2019 version of the GTP addin in 64bit mode
rem
rem              build 2023 -32
rem
rem           -86 and -32 are the same (they build 32 bit)
rem
rem  3 Sept 2023
rem  Code Kill
rem  https://github.com/srives/GTPRevitToolkit
rem
rem -----------------------------------------------------------------------


rem ---------------------- Configure your MSBuild Here --------------------
set VSVER=2022
set MSBUILD=C:\Program Files\Microsoft Visual Studio\2022\Professional\Msbuild\Current\Bin\MSBuild.exe
if not exist "%MSBUILD%" set VSVER=2019
if not exist "%MSBUILD%" set MSBUILD=C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe
if not exist "%MSBUILD%" set VSVER=2017
if not exist "%MSBUILD%" set MSBUILD=C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe
if not exist "%MSBUILD%" echo Could not find MSBuild.exe. Change your build.cmd script.
if not exist "%MSBUILD%" goto :EOF

rem ---------------------- Command Line Parameters  ------------------------
	set BIT=64
	set LOOP=0
	set WHICH=Both
	:LOOP_TOP
      set /A LOOP=LOOP+1
	  if /I (%1)==(-Release) set WHICH=Release
	  if /I (%1)==(-Release) shift
	  if /I (%1)==(-Debug) set WHICH=Debug
	  if /I (%1)==(-Debug) shift
	  if /I (%1)==(-64) set BIT=64
	  if /I (%1)==(-64) shift
	  if /I (%1)==(-32) set BIT=86
	  if /I (%1)==(-32) shift
	  if /I (%1)==(-86) set BIT=86
	  if /I (%1)==(-86) shift
	  if /I (%1)==(-x64) set BIT=64
	  if /I (%1)==(-x64) shift
	  if /I (%1)==(-x32) set BIT=86
	  if /I (%1)==(-x32) shift
	  if /I (%1)==(-x86) set BIT=86
	  if /I (%1)==(-x86) shift
	if not (%LOOP%) == (3) goto :LOOP_TOP

rem ------------------------------ MAIN -------------------------------------

    echo Building GTP Revit Toolkit with Visual Studio version %VSVER%
	if (%WHICH%)==(Both) echo Building both DEBUG and RELEASE (use -Release or -Debug to build only one).
	if (%1)==() echo Building toolkit to work with all version of Revit: 2019,2020,2021,2022,2023 and 2024
	if (%1)==() echo          (use -2023 to build only 2023, or -2022 to build only for Revit 2022, etc.)
	if not (%1)==() echo Building toolkit to work with only Revit %1
    if (%BIT%)==(64) echo Buidling 64bit (use -32 to build 32bit)
    if (%BIT%)==(86) echo Buidling 32bit

    cd ..
	if not (%1)==() call :Build %1
	if not (%1)==() goto :DONE

	call :Build 2019
	call :Build 2020
	call :Build 2021
	call :Build 2022
	call :Build 2023
	call :Build 2024
	
	:DONE
	cd install
    if (%WHICH%)==(Release)       echo To debug, run: build -dev
    if not (%WHICH%)==(Release)   echo Now run: install --dev 
    if not (%WHICH%)==(Release)   echo          This will create .addin files for each version of the GTP Toolkit on your own local machine.
	
goto :EOF


rem ------------------------ Subroutine: Build() ---------------------------
:Build
    echo Building %1 x%BIT%

:Debug
    if (%WHICH%)==(Release) goto :Release
    echo. > debug%1.txt
    del .\bin\%1\Debug\GTPRevitToolkit.dll 1>nul 2>nul
	"%MSBUILD%" GTPRevitToolkit.sln "/property:Configuration=Debug (Revit %1)" /p:Platform=x%BIT% /p:DefineConstants="Revit%1" /p:WarningLevel=0 >> debug%1.txt
    if exist ".\bin\x%BIT%\Debug (Revit %1)\GTPRevitToolkit.dll"       echo          %1 Debug   build: SUCCESS
    if not exist ".\bin\x%BIT%\Debug (Revit %1)\GTPRevitToolkit.dll"   echo          %1 Debug   build: FAILED to create bin\x%BIT%\Debug (Revit %1)\Debug\GTPRevitToolkit.dll
    if not exist ".\bin\x%BIT%\Debug (Revit %1)\GTPRevitToolkit.dll"   type debug%1.txt | find "Error"
	del debug%1.txt 1>nul 2>nul
		
:Release
    if (%WHICH%)==(Debug) goto :EOF	
    del .\bin\%1\Release\GTPRevitToolkit.dll 1>nul 2>nul	
    echo. > release%1.txt
	"%MSBUILD%" GTPRevitToolkit.sln "/property:Configuration=Release (Revit %1)" /p:Platform=x%BIT% /p:DefineConstants="Revit%1" /p:WarningLevel=0 >> release%1.txt
    if exist ".\bin\x%BIT%\Release (Revit %1)\GTPRevitToolkit.dll"     echo          %1 Release build: SUCCESS
    if not exist ".\bin\x%BIT%\Release (Revit %1)\GTPRevitToolkit.dll" echo          %1 Release build: FAILED to create bin\x%BIT%\Release (Revit %1)\GTPRevitToolkit.dll
	if not exist ".\bin\x%BIT%\Release (Revit %1)\GTPRevitToolkit.dll" type release%1.txt | find /I "error"
    rem | find "Error"
	del release%1.txt 1>nul 2>nul
	
goto :EOF
