@echo off


rem -------------------------------------------------------
rem   Sign the DLLs built for GTP Toolkit
rem   This is not essential, but signed files are easier
rem   to distribute
rem
rem   Options:
rem
rem     -32:      To sign 32 bit DLLS
rem     -64:      To sign 64 bit DLLS (default)
rem     -Release: To sign Release DLLS
rem     -Debug:   To sign Debug DLLS
rem     -Both:    To sign both Release and Debug DLLs (default) 
rem       
rem   3 Sept 2023
rem   Code Kill
rem   https://github.com/srives/GTPRevitToolkit
rem
rem -------------------------------------------------------

:CONFIG
	set signFile=C:\repos\Stratus\Certificates\GTPServices.pfx
	rem We need a password for the cert. We get it from this sign_pw.cmd, but however you get it, set pw=password
	set pwFile=C:\repos\secrets\sign_pw.cmd
	
	if not exist "%signFile%" set signFile=C:\repos\Stratus\Certificates\GTPServices,LLC.pfx
	set signTool=C:\Program Files (x86)\Windows Kits\10\App Certification Kit\signtool.exe

	rem Source Path is the place under which there is the gtp-toolkit.sln file
	if "%SOURCEPATH%"=="" set SOURCEPATH=C:\repos\GTP\GTPRevitToolkit

rem ----------------------------------------------------------------------------------------------
:PARSE_COMMAND_LINE
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

rem ----------------------------------------------------------------------------------------------
:MAIN
	if not exist "%signTool%" echo Not Signing DLLs, missing the signtool.exe file that is in the Windows Kits.
	if not exist "%pwFile%" echo Not Signing DLLs, missing password file %pwFile%, which should have a on line "set pw=" command in it.
	if not exist "%signFile%" echo Not Signing DLLs, missing PFX signature file.
	if not exist "%signFile%" goto :EOF
	if not exist "%pwFile%" goto :EOF
	if not exist "%signTool%" goto :EOF
	call %pwFile%

	echo Sign all EXEs we place on the client computer
	echo About to sign GTP Revit Toolkit

	call :sign_it 2019
	call :sign_it 2020
	call :sign_it 2021
	call :sign_it 2022
	call :sign_it 2023
	call :sign_it 2024
    
goto :EOF

rem ----------------------------------------------------------------------------------------------
:sign_it
rem SIGN Debug EXE 

:DEBUG
    if (%WHICH%)==(Release) goto :RELEASE
	if not exist "%SOURCEPATH%\bin\x%BIT%\Debug (Revit %1)\GTPRevitToolkit.dll" goto :RELEASE
	echo SIGN Debug DLL for %1
	"%signTool%" sign /td SHA256 /fd SHA256 /f %signFile% /p %pw% /tr http://timestamp.digicert.com/ "%SOURCEPATH%\bin\x%BIT%\Debug (Revit %1)\GTPRevitToolkit.dll"

:RELEASE
    if (%WHICH%)==(Debug) goto :EOF
	if not exist "%SOURCEPATH%\bin\x%BIT%\Release (Revit %1)\GTPRevitToolkit.dll" goto :EOF
	echo SIGN Release DLL for %1
	"%signTool%" sign /td SHA256 /fd SHA256 /f %signFile% /p %pw% /tr http://timestamp.digicert.com/ "%SOURCEPATH%\bin\x%BIT%\Release (Revit %1)\GTPRevitToolkit.dll"

goto :EOF