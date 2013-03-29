@echo off
set resultDir=..\results
if not exist %resultDir% mkdir %resultDir%

..\tools\nant\nant.exe -buildfile:nant.build -l:%resultDir%\Nant-Build.log integration-build 
if ERRORLEVEL 1 goto Failed

..\tools\nant\nant.exe -buildfile:nant.build run-fxcop
if ERRORLEVEL 1 goto Failed

echo "compilation and unit testing completed. Log file and unit-tests results are stored in %resultDir%"
goto End


:Failed
echo "Failed"


:End
pause
