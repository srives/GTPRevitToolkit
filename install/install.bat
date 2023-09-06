@echo off

goto :Main

:Usage
echo.
echo Usage: install [-?] [-dev] [-rel] [-32] [-64]
echo        CodeK ill install script for GTP Revit ToolKit 
echo.
echo  This script runs inside an installer, but it can also be run outside of the installer on the 
echo  dev machine.
echo.
echo  This batch file gets bundled with the installer for GTP Revit Toolkit 
echo  This script is meant to be included in a self-extracting EXE, and run after decompression
echo.
echo  Developer Notes:
echo.
echo         You can run:
echo.
echo                          install -dev
echo.
echo         Assumes you first ran:
echo.
echo                          build
echo.
echo         And it will use the development machine build path as the source of the
echo         revit manifest files (a developer would do this while building/debugging.
echo.
echo                          install -dev -32
echo.
echo         And it will use the development machine build path as the source of the
echo         revit manifest files (a developer would do this while building/debugging)
echo         and use your 32bit version.of your build
echo.
echo         You can run this instead of running, install -dev:
echo.
echo                          install --rel
echo.
echo         Running this will point Revit to the locally built release DLLs
echo.
echo  To check install results, check this file:
echo.
echo         C:\Program Files (x86)\GTP Software, Inc\GTPRevitToolkit\install.txt
echo.
echo  3 Sept 2023
echo  Code Kill
echo  https://github.com/srives/GTPRevitToolkit
echo.
rem ----------------------------------------------------------------------------------------------
goto :EOF


:Main
rem --------------------- Install these DLLS -------------------------
  set WHAT=Release
  set BIT=64



  rem we cannot use %cd% as it is used in the installer packages, like PDQ, and gets destroyed
  set cwd=
  for /F "tokens=*" %%a in ('dir install.bat /b /s') do set cwd=%%~dpa
  cd "%cwd%"
  rem cwd ends with \
 
  set UnzipPath=%cwd%
  echo Current Directory = %cwd%
  set GTPRoot=C:\Program Files (x86)\GTP Software, Inc\GTPRevitToolkit
  
  set DEV=0
  set LOOP=0
  :LOOP_TOP  
      set /A LOOP=LOOP+1
	  if /I (%1)==(-dev) set DEV=1
	  if /I (%1)==(-dev) set WHAT=Debug
	  if /I (%1)==(-dev) shift
	  if /I (%1)==(--dev) set DEV=1
	  if /I (%1)==(--dev) set WHAT=Debug
	  if /I (%1)==(--dev) shift
	  if /I (%1)==(--rel) set DEV=1
	  if /I (%1)==(--rel) set WHAT=Release
	  if /I (%1)==(--rel) shift
	  if /I (%1)==(-rel) set DEV=1
	  if /I (%1)==(-rel) set WHAT=Release
	  if /I (%1)==(-rel) shift
  	  if /I (%1)==(-64) set BIT=64
	  if /I (%1)==(-64) shift
  	  if /I (%1)==(--64) set BIT=64
	  if /I (%1)==(--64) shift
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
	  if /I (%1)==(-?) goto :USAGE
	  if /I (%1)==(--?) goto :USAGE
	  if /I (%1)==(--h) goto :USAGE
	  if /I (%1)==(-h) goto :USAGE
  if not (%LOOP%) == (3) goto :LOOP_TOP

  if (%DEV%)==(1) set GTPRoot=C:\repos\GTP\GTPRevitToolkit\bin\x%BIT%
  if (%DEV%)==(1) if (%BIT%)==(86) echo Debugging 32bit DLLs (%WHAT%)
  if (%DEV%)==(1) if (%BIT%)==(64) echo Debugging 64bit DLLs (%WHAT%)
  
  mkdir "%GTPRoot%" 1>nul 2>nul
  set installLog=%GTPRoot%\install.txt
  echo. >> "%installLog%"  
  echo. >> "%installLog%"
  echo ------------------------------------------------- >> "%installLog%"
  echo Running GTP Revit Toolkit Installer >> "%installLog%"
  date /T >> "%installLog%"
  time /T >> "%installLog%"
  
  echo ------------------------------------------------- >> "%installLog%"
  set >> "%installLog%"
  echo ------------------------------------------------- >> "%installLog%"
  echo Current Directroy >> "%installLog%"
  echo %cwd%  >> "%installLog%"
  tree /A /F >> "%installLog%"
  echo ------------------------------------------------- >> "%installLog%"

  echo Installer Contents: >> "%installLog%"
  echo ------------------------------------------------- >> "%installLog%"
  dir "%UnzipPath%" /s /b >> "%installLog%"
  echo ------------------------------------------------- >> "%installLog%"

  echo Putting GTP Toolkit files here: %GTPRoot% >> "%installLog%"
    
  rem Determine Autodesk Path, it can be one of three places  
  set adpath=C:\ProgramData\Autodesk\Revit\Addins\
  if exist "%adpath%" goto :ready

  set adpath=C:\%USERNAME%\AppData\Roaming\Autodesk\Revit\Addins\
  if exist "%adpath%" goto :ready
  
  set adpath=C:\%USERNAME%\AppData\Roaming\Autodesk\ApplicationPlugins\
  if exist "%adpath%" goto :ready
  
  set adpath=C:\ProgramData\Autodesk\Revit\Addins\
  echo ERROR: Could not find Autodesk path: %adpath%, we will just use the most common one
  echo ERROR: Could not find Autodesk path: %adpath%, we will just use the most common one >> "%installLog%"
  
:ready

  echo Autodesk Revit Addin Location: %adpath%
  echo Autodesk Revit Addin Location: %adpath% >> "%installLog%"

  call :RevitYear 2019
  call :RevitYear 2020
  call :RevitYear 2021
  call :RevitYear 2022
  call :RevitYear 2023
  call :RevitYear 2024

  echo ------------------------------------------------ >> "%installLog%"
  echo GTP Revit Toolkit Install finished >> "%installLog%"
  echo Finished. Check install log: %installLog%  
  echo.  >> "%installLog%"

  rem ----------------------------------------------------------------------------------------------  
  if (%DEV%)==(1) echo.
  if (%DEV%)==(1) echo Debug Help. To run Revit against the debugger in Visual Studio
  if (%DEV%)==(1) echo             (using Revit 2020 as an example) go to Properties, then Debug and set
  if (%DEV%)==(1) echo             "Start External Program" to C:\Program Files\Autodesk\Revit 2020\Revit.exe
  if (%DEV%)==(1) echo.
  if (%DEV%)==(1) if (%BIT%)==(64) echo             To debug 32bit DLLs, use:
  if (%DEV%)==(1) if (%BIT%)==(64) echo                    install -dev -32
  
  echo Install Log: %installLog%

goto :EOF

rem ----------------------------------------------------------------------------------------------
:RevitYear
rem For the passed in year, deposit the GTP related toolkit DLLs, and create a Revit manifest.
rem Copy DLLs from installer over to the user's C:\Program Files (x86)\GTP Software, Inc\GTPRevitToolkit
rem directory
rem ----------------------------------------------------------------------------------------------

  set DLLPATH=%GTPRoot%

  echo ------------------- %1 ------------------ >> "%installLog%"
  
  rem If we are DEV=1, then we don't copy the DLLs over to any place (we run them out of the build dir)
  rem 
  rem In case of DEV:C:\repos\GTP\GTPRevitToolkit\bin\x64\Debug (Revit 2023)
  if (%DEV%)==(1) set DLL=%DLLPATH%\%WHAT% (Revit %1)\GTPRevitToolkit.dll
  if (%DEV%)==(1) echo Running Developer Mode (%WHAT% DLLS)
  if (%DEV%)==(1) echo Running Developer Mode (%WHAT% DLLS) >> "%installLog%"
  if (%DEV%)==(1) goto :make_manifest

  rem We copy all the GTP Revit Addin DLLs to the GTPRoot--e.g., to C:\Program Files (x86)\GTP Software, Inc\GTPRevitToolkit\2023\
  set instdir=%DLLPATH%\%1
  set DLL=%instdir%\%WHAT%\GTPRevitToolkit.dll
  echo Installing GTP Revit Toolkit for Revit %1
  echo Installing GTP Revit Toolkit for Revit %1 >> "%installLog%"

  rem Copy Files  
  mkdir "%instdir%\" 1>nul 2>nul
  del "%instdir%\*.*" /s /q 1>nul 2>nul
  echo  xcopy "%UnzipPath%%1\*.*" "%instdir%\" /s /q /y 
  echo  xcopy "%UnzipPath%%1\*.*" "%instdir%\" /s /q /y >> "%installLog%"
  xcopy "%UnzipPath%%1\*.*" "%instdir%\" /s /q /y >> "%installLog%"

  if not exist "%instdir%\%WHAT%\GTPRevitToolkit.dll" echo Cannot find "%instdir%\%WHAT%\GTPRevitToolkit.dll" >> "%installLog%"
  if not exist "%instdir%\%WHAT%\GTPRevitToolkit.dll" echo Cannot find "%instdir%\%WHAT%\GTPRevitToolkit.dll"
  if not exist "%instdir%\%WHAT%\GTPRevitToolkit.dll" echo xcopy "%UnzipPath%%ZipTop%\%1\*.*" "%instdir%\" /s /q /y Failed
  rem if not exist "%instdir%\%WHAT%\GTPRevitToolkit.dll" if "%USERNAME%"=="xxxxxxxxxxxxxxxxxxxx" pause
  
  
  rem -----------------------------------------------------------------------------
  :make_manifest
  rem Create Manifest
  mkdir %adpath%%1 1>nul 2>nul
  set manifest=%adpath%%1\GTPRevitToolkit%1.addin
  echo Creating %1 manifest, pointing to %DLL%
  echo Creating %1 manifest, pointing to %DLL% >> "%installLog%"
  
      echo ^<?xml version="1.0" encoding="utf-8" ?^> > "%manifest%" 2>nul
      echo ^<RevitAddIns^> >> "%manifest%"
      echo   ^<AddIn Type="Application"^> >> "%manifest%"
      echo     ^<Name^>GTP Toolkit^</Name^> >> "%manifest%"
      echo     ^<Description^>GTP Toolkit Ribbon for Revit %1^</Description^> >> "%manifest%"
      echo     ^<Assembly^>%DLL%^</Assembly^> >> "%manifest%"
      echo     ^<FullClassName^>GTP.Application^</FullClassName^> >> "%manifest%"
      echo     ^<ClientId^>ccacccec-ccc1-2ccc-bcc3-4cccca5bcec6^</ClientId^> >> "%manifest%"
      echo     ^<VendorId^>GTPX^</VendorId^> >> "%manifest%"
      echo     ^<VendorDescription^>GTP Software, Inc.^</VendorDescription^> >> "%manifest%"
      echo   ^</AddIn^> >> "%manifest%"
      echo ^</RevitAddIns^> >> "%manifest%"
  
  if exist "%manifest%" echo Created Manifest %manifest%.  >> "%installLog%"
  if not exist "%manifest%" echo Failed to created Manifest %manifest% (you do not have Revit %1 installed).
  if not exist "%manifest%" echo Failed to created Manifest %manifest% (you do not have Revit %1 installed).  >> "%installLog%"
  
goto :EOF
