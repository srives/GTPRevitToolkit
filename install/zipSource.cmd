@echo off
rem -------------------------------------------------------------------
rem
rem     Written for GTP, for a GTP Revit Toolkit
rem     This will delete the binaries, then ZIP up the source code
rem     so that I can send it to GTP so we can archive it.
rem
rem     3 Sept 2023
rem     Code Kill
rem     https://github.com/srives/GTPRevitToolkit
rem
rem -------------------------------------------------------------------

cd..	
set SOURCEPATH=%cd%
set SOURCE_ZIP=%SOURCEPATH%-source-%TODAY%.zip
rem TODAY=Year-Month-Day
for /F "tokens=1-5 delims=/ " %%i in ('date /t') do set TODAY=%%l-%%j-%%k

cd install
call nogtp.cmd

del "%SOURCE_ZIP%" 1>nul 2>nul
powershell Compress-Archive "%SOURCEPATH%" -DestinationPath "%SOURCE_ZIP%"

echo.
echo Your Sourcecode is zipped here: "%SOURCE_ZIP%"
echo Upload to GTP Sharepoint here:
echo.

