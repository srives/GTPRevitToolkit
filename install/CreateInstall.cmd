@echo off

rem -------------------------------------------------------------------
rem     This batch script will take your source code 
rem     delete the build, and rebuild it. It will then
rem     create an EXE file that is an installer.
rem
rem     This program uses the paid version of WinZip
rem     that turns ZIPs into EXE files.
rem
rem     ----------------------------------------------------
rem     Options:
rem     ----------------------------------------------------
rem     -D:  To Create a Debug version of the installer, use:
rem             CreateInstall -D
rem          By default, creates a RELEASE version of the DLLS.
rem
rem     -S:  To create a ZIP copy of the source code:
rem             CreateInstall -S
rem
rem     -R:  To run the install after it is created
rem             CreateInstall -R
rem             Warning: this replaces your manifests files
rem
rem     -64:  Build install package as 64bit (default)
rem             CreateInstall -64
rem
rem     -86:  Build install package as 32bit
rem             CreateInstall -86
rem
rem     -32:  Build install package as 32bit
rem             CreateInstall -32
rem
rem     -NO: Don't build. 
rem          You'd do this if you are testing only installer changes
rem
rem     2019:
rem         Just build an installer for 2019 only
rem
rem     2020:
rem         Just build an installer for 2020 only
rem
rem     2021:
rem         Just build an installer for 2021 only
rem
rem     2022:
rem         Just build an installer for 2022 only
rem
rem     2023:
rem         Just build an installer for 2023 only
rem
rem     2024:
rem         Just build an installer for 2024 only
rem
rem
rem     Written for GTP, a Revit Toolkit
rem     3 Sept 2023
rem     Code Kill
rem     https://github.com/srives/GTPRevitToolkit
rem
rem -------------------------------------------------------------------


echo %0 Code Kill, Installer adapted for GTP Revit Toolkit
echo -----------------------------------------------------------------------

:ENV_VARS
    cd..	
	set SOURCEPATH=%cd%
	cd install
	set Text=%SOURCEPATH%\install\Install.message.txt
	set About=%SOURCEPATH%\install\Install.message.txt
	set Template=%SOURCEPATH%\install\InstallTitle.txt
    rem We will STAGE the files for the installer to c:\AppName
	set AppName=GTPRevitToolkit
    set MainDLL=%AppName%.dll
    rem We use a tool that turns a ZIP into an EXE (it is installed here):
	set WZIP=C:\Program Files (x86)\WinZip Self-Extractor\WZIPSE32.EXE
	set signTool=C:\Program Files (x86)\Windows Kits\10\App Certification Kit\signtool.exe
	set signFile=C:\repos\Stratus\Certificates\GTPServices.pfx
	if not exist "%signFile%" set signFile=C:\repos\Stratus\Certificates\GTPServices,LLC.pfx

    rem TODAY=Year-Month-Day
    for /F "tokens=1-5 delims=/ " %%i in ('date /t') do set TODAY=%%l-%%j-%%k

rem -------------------------------------------------------------------
:COMMAND_LINE_OPTIONS
	set WHAT=Release
	set ZIP_SOURCE=0
	set RUN_INSTALL=0
	set LOOP=0
	set DOBUILD=1
	set BIT=64
	:LOOP_TOP
      set /A LOOP=LOOP+1
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
	  if /I (%1)==(-D) set WHAT=Debug
	  if /I (%1)==(-D) shift
	  if /I (%1)==(-S) set ZIP_SOURCE=1
	  if /I (%1)==(-S) shift
	  if /I (%1)==(-R) set RUN_INSTALL=1
	  if /I (%1)==(-R) shift
	  if /I (%1)==(-NO) set DOBUILD=0
	  if /I (%1)==(-NO) shift
	if not (%LOOP%) == (4) goto :LOOP_TOP

    set INSTALLER=%0
	set SHOW=64
	if (%BIT%)==(86) set SHOW=32
	if (%BIT%)==(32) set SHOW=32

	rem We create a subfolder of same name under the folder
	set StageRoot=c:\%AppName%\%AppName%
	set BaseFile=%StageRoot%%SHOW%bit
	rem Note well: the EXE file name must be same name as the ZIP file (except by extension)
	set ZipFile=%BaseFile%.zip
	set EXEFile=%BaseFile%.exe

rem -------------------------------------------------------------------
:CLEAN
	if (%DOBUILD%)==(0) echo Not cleaning, because we are not building. 
	if (%DOBUILD%)==(0) goto :ZIPSOURCE

	echo Removing previous build binaries from %SOURCEPATH%\bin\, and \obj\
	rd /s /q "%SOURCEPATH%\bin\" 1>nul 2>nul
	rd /s /q "%SOURCEPATH%\obj\" 1>nul 2>nul

rem -------------------------------------------------------------------
:ZIPSOURCE
	set SOURCE_ZIP=%SOURCEPATH%-source-%TODAY%.zip
	if (%ZIP_SOURCE%)==(0) goto :BUILD

	rem E.g., SOURCE_ZIP=C:\repos\GTP\GTPRevitToolkit-2023-09-02

	echo Creating Source CODE Zip at %SOURCE_ZIP%
	del "%SOURCE_ZIP%" 1>nul 2>nul
	if exist "%SOURCE_ZIP%" echo You have %SOURCE_ZIP% open. Close it.
	echo powershell Compress-Archive "%SOURCEPATH%" -DestinationPath "%SOURCE_ZIP%"
	powershell Compress-Archive "%SOURCEPATH%" -DestinationPath "%SOURCE_ZIP%"

rem -------------------------------------------------------------------
:BUILD
	if (%DOBUILD%)==(0) echo Not Building (-NO was passed in) 
	if (%DOBUILD%)==(0) goto :POST_BUILD

	echo Building GTP Revit Toolkit
	echo Creating %WHAT% version of the installer (pass in -D to this script to create the DEBUG version of the installer)
	rem Passing in a parameter to build.cmd just limits the build to the year passed into this script
	call .\build.cmd -%BIT% -%WHAT% %1

rem -------------------------------------------------------------------
:POST_BUILD
  set found=0
  call :CHECK 2019
  call :CHECK 2020
  call :CHECK 2021
  call :CHECK 2022
  call :CHECK 2023
  call :CHECK 2024
  
  if (%found%)==(0) echo No files found to create install package. Missing DLL %MainDLL% for all versions of Revit %SHOW%bit %WHAT%
  if (%found%)==(0) goto :EOF  
  goto :SIGN_DLLS

  rem -------------------------------------------------------------------
  rem -- Subroutine to CHECK to make sure we have something to install --
  :CHECK
	  if not exist "%SOURCEPATH%\bin\x%BIT%\%WHAT% (Revit %1)\%MainDLL%"   echo GTP Revit Toolkit is Missing %WHAT% of Revit %1 %SHOW%bit %MainDLL%
	  if exist "%SOURCEPATH%\bin\x%BIT%\%WHAT% (Revit %1)\%MainDLL%"       set found=1
	  if exist "%SOURCEPATH%\bin\x%BIT%\%WHAT% (Revit %1)\%MainDLL%"       echo Including Revit %1 %WHAT% %SHOW%bit for GTP Revit Toolkit in Installer
  goto :EOF


rem -------------------------------------------------------------------
:SIGN_DLLS
  echo Signing all relevant DLL files (not necessary for the installer, optional)
  call .\sign.cmd -%BIT% -%WHAT%
goto :CREATE_STAGE


rem -------------------------------------------------------------------
:CREATE_STAGE
	echo Create staging area at c:\%AppName%\ where we will build our ZIP and EXE
	echo Erase previous version of the staged files
	rd "%StageRoot%\" /s /q 1>nul 2>nul

	mkdir "%StageRoot%\" 1>nul 2>nul
	mkdir "%StageRoot%\2019" 1>nul 2>nul
	mkdir "%StageRoot%\2020" 1>nul 2>nul
	mkdir "%StageRoot%\2021" 1>nul 2>nul
	mkdir "%StageRoot%\2022" 1>nul 2>nul
	mkdir "%StageRoot%\2023" 1>nul 2>nul
	mkdir "%StageRoot%\2024" 1>nul 2>nul

	echo Ready to create GTP Revit Install at c:\%AppName%\

rem -------------------------------------------------------------------
:STAGE
	echo Copying all %WHAT% files to ZIP dir for compression
	copy "%SOURCEPATH%\install\install.bat" "%StageRoot%\" 1>nul 2>nul
	if not exist "%StageRoot%\install.bat" echo ERROR: could not copy install.bat file to %StageRoot%\install.bat
	if not exist "%StageRoot%\install.bat" goto :EOF
	 
	rem ------------ Check for files NOT to copy to the ZIP ---------------
	set EXCLUDE=
	if exist "%SOURCEPATH%\install\excludeFiles.txt" echo Excluding the following files from staging:
	if exist "%SOURCEPATH%\install\excludeFiles.txt" type "%SOURCEPATH%\install\excludeFiles.txt"
	if exist "%SOURCEPATH%\install\excludeFiles.txt" echo.
	if exist "%SOURCEPATH%\install\excludeFiles.txt" set EXCLUDE=/EXCLUDE:excludeFiles.txt

	echo Current Directory: %cd%
	if not (%1)==() call :STAGE_BY_YEAR %1
	if not (%1)==() echo Files for year %1 *ONLY* are ready to be turned into a Zip
	if not (%1)==() goto :ZIP

		call :STAGE_BY_YEAR 2019
		call :STAGE_BY_YEAR 2020
		call :STAGE_BY_YEAR 2021
		call :STAGE_BY_YEAR 2022
		call :STAGE_BY_YEAR 2023
		call :STAGE_BY_YEAR 2024

	echo Files ready to be turned into a Zip
goto :ZIP

rem ------------------------ Subroutine -----------------------------------
:STAGE_BY_YEAR
  echo Stage %SHOW%bit Revit %1 %WHAT% addin: %StageRoot%\%1\%WHAT%\
  echo xcopy "%SOURCEPATH%\bin\x%BIT%\%WHAT% (Revit %1)\*.*" "%StageRoot%\%1\%WHAT%\"
  xcopy "%SOURCEPATH%\bin\x%BIT%\%WHAT% (Revit %1)\*.*" "%StageRoot%\%1\%WHAT%\" %EXCLUDE% /S /Q /Y  2>nul
  if not exist "%StageRoot%\%1\%WHAT%\" echo There is no build for %SHOW%bit Revit %1 %WHAT% 
  if not exist "%StageRoot%\%1\%WHAT%\" goto :EOF

  :CREATE_README  
	  rem -----------------------------------------------------------------  
	  rem Ceate a unique Readme file that gets deposited in each directory
	  rem This is so we can maybe diagnose any issues on a particular 
	  rem install of Revit. This readme file will be copied to the 
	  rem target machine to this directory (in the case of Revit 2023):
	  rem
	  rem    C:\Program Files (x86)\GTP Software, Inc\%AppName%\2023\
	  rem
	  set locallog=%StageRoot%\%1\%WHAT%\-----Revit%1-%SHOW%bit-%WHAT%----.readme
	  echo Packaged by GTP %INSTALLER% Installer. Created on %TODAY% > %locallog%
	  echo Built on COMPUTERNAME=%COMPUTERNAME% with MSBUILD=%VSVER% >> %locallog%
	  echo GTP Revit Toolkit, %SHOW%bit %WHAT%, Revit %1 >> %locallog%
	  if (%SHOW%)==(64) echo You ran the 64 bit GTP Revit Toolkit install (GTP Services, with GTP) >> %locallog%
	  if (%SHOW%)==(32) echo This is the 32 bit GTP Revit Toolkit install (GTP Services, with GTP) >> %locallog%
	  if not exist "%StageRoot%\%1\%WHAT%\%MainDLL%" echo WARNING: The %MainDLL% was not built. >> %locallog%
	  if not exist "%StageRoot%\%1\%WHAT%\%MainDLL%" goto :EOF
	  echo Directory of .\bin\x%BIT%\%WHAT% (Revit %1)\*.* >> %locallog%
	  dir /s "%SOURCEPATH%\bin\x%BIT%\%WHAT% (Revit %1)\*.*" >>  %locallog%
	  
  :RELEASE_NOTES
     copy "%SOURCEPATH%\ReleaseNotes.txt" "%StageRoot%\%1\%WHAT%\ReleaseNotes.txt" 1>nul 2>nul
goto :EOF

rem -------------------------------------------------------------------
:ZIP
echo Creating Zip: %ZipFile%
rem Delete the old copy
  del "%ZipFile%" 1>nul 2>nul
  if exist "%ZipFile%" echo ERROR: you have "%ZipFile%" open. Close it.

  powershell Compress-Archive "%StageRoot%" "%ZipFile%"
  
  if not exist "%EXEFile%" goto :EXE
  mkdir "c:\%AppName%\previous\" 2>nul
  echo Backing up old installer
  copy "%EXEFile%" "c:\%AppName%\previous\%AppName%x%BIT%.%RANDOM%.exe" 1>nul

rem -------------------------------------------------------------------
:EXE
	echo Creating Self-Extracting EXE 
	echo Checking to see if we have the WinZip EXE Creation Tool
	if not exist "%WZIP%" echo Cannot create self-extracting EXE (missing %WZIP%)
	if not exist "%WZIP%" echo Your install package is in a ZIP called, %ZipFile%
	if not exist "%WZIP%" echo To install, unzip that file and run install.bat
	if not exist "%WZIP%" goto :EOF

	type %Template% > %Text%
	echo Version: %TODAY% >> %Text%

	echo Running WinZip Self Extraction Creation tool (consider getting licensed version to avoid unwanted annoy messages)
	"%WZIP%" %ZipFile% -auto -setup -t %Text% -a %About% -c .\%AppName%\install.bat

	if not exist "%EXEFile%" echo Your self extracting EXE (%EXEFile%) failed to get created.
	if not exist "%EXEFile%" echo The following command failed:
	if not exist "%EXEFile%" echo "%WZIP%" %ZipFile% -auto -setup -t %Text% -a %About% -c .\%AppName%\install.bat
	if not exist "%EXEFile%" goto :EOF

rem -------------------------------------------------------------------
:SIGN
	echo Signing Installer EXE file (this is not required, if it doesn't get signed, the installer will work still)

	if not exist "%signTool%" echo Not Signing Installer, missing the signtool.exe file that is in the Windows Kits.
	if not exist "%pwFile%" echo Not Signing Installer, missing password file %pwFile%, which should have a on line "set pw=" command in it.
	if not exist "%signFile%" echo Not Signing Installer, missing PFX signature file.
	if not exist "%signFile%" goto :RUN
	if not exist "%pwFile%" goto :RUN
	if not exist "%signTool%" goto :RUN
	call %pwFile%

	rem Sign the self-extracting EXE file
	"%signTool%" sign /td SHA256 /fd SHA256 /f "%signFile%" /p %pw% /tr http://timestamp.digicert.com/ %EXEFile%

rem -------------------------------------------------------------------
:RUN
	if (%RUN_INSTALL%)==(0) goto :FINAL_NOTE
	echo.
	echo --------------------- Running Install Program --------------------
	echo          Running: %EXEFile%
	echo Warning: this replaces your manifests files
	echo          Run install.bat -dev to reset your manifests for dev testing
	"%EXEFile%"
	echo          Ran the latestet instance of the GTP toolkit installer.
	echo          Check Install log here: CC:\Program Files (x86)\GTP Software, Inc\%AppName%\install.txt

rem -------------------------------------------------------------------
:FINAL_NOTE
	echo.
	if (%ZIP_SOURCE%)==(1) echo Your SOURCE CODE is zipped here: %SOURCE_ZIP% (send this to GTP)
	if (%ZIP_SOURCE%)==(1) goto :FINI
	
	:HOW_TO_ZIP_SOURCE_MANUALLY
	echo If you want to create a ZIP file of the source code, re-run as:
	echo.
	echo     CreateInstall -S
	echo.
	echo OR, to create the zip of the source code now, run the following two steps:
	echo.
	echo       nogtp.cmd (to cleanup all the binaries)
	echo       powershell Compress-Archive "%SOURCEPATH%" -DestinationPath "%SOURCE_ZIP%"
	echo.	
	
:FINI
echo Your INSTALLER is: %EXEFile%
echo Copying installer to ..
copy "%EXEFile%" ..
echo.
