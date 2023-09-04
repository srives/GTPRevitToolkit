@echo off
rem -------------------------------------------------------------------
rem
rem Uninstall the GTP toolkit from all versions of Revit
rem This is useful during testing
rem
rem     3 Sept 2023
rem     CodeKill
rem     https://github.com/srives/GTPRevitToolkit
rem
rem -------------------------------------------------------------------

  set GTPRoot=C:\Program Files (x86)\GTP Software, Inc\GTPRevitToolkit
  set WHAT=Release
  set ZipTop=GTPRevitToolkit

  set adpath=C:\ProgramData\Autodesk\Revit\Addins\
  if exist "%adpath%" goto :ready

  set adpath=C:\%USERNAME%\AppData\Roaming\Autodesk\Revit\Addins\
  if exist "%adpath%" goto :ready
  
  set adpath=C:\%USERNAME%\AppData\Roaming\Autodesk\ApplicationPlugins\
  if exist "%adpath%" goto :ready
  
  set adpath=C:\ProgramData\Autodesk\Revit\Addins\
  echo ERROR: Could not find Autodesk path: %adpath%, we will just use the most common one 
  
:ready

  echo Delete Build files (if you want them back, just run build.cmd)
  rd "..\bin\" /s /q 1>nul 2>nul
  rd "..\obj\" /s /q 1>nul 2>nul
  echo Delete Staging files (if you want them back, just run CreateInstall.cmd)
  rd "c:\%ZipTop%\%ZipTop%\" /s /q 1>nul 2>nul

  echo Removing WHOLE directory: "%GTPRoot%"
  rd "%GTPRoot%" /s /q 1>nul 2>nul
  
  echo Autodesk Revit Addin Location: %adpath%
  
  call :RevitYear 2018
  call :RevitYear 2019
  call :RevitYear 2020
  call :RevitYear 2021
  call :RevitYear 2022
  call :RevitYear 2023
  call :RevitYear 2024
  
  goto :EOF


:RevitYear
    echo.
    
	set instdir=%GTPRoot%\%1
    if exist "%instdir%\" echo Delete installed binaries for %1 (%instdir%\)		
	rd "%instdir%\" /s /q 1>nul 2>nul
		
	set manifest=%adpath%\%1\GTPRevitToolkit%1.addin
	if not exist "%manifest%" echo GTP Revit %1 Toolkit Addin Manifest NOT FOUND
	if exist "%manifest%"     echo GTP Revit %1 Toolkit Addin Manifest DELETED
	if exist "%manifest%" del %manifest% 1>nul 2>nul
	
goto :EOF