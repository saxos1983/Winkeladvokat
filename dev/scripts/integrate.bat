@echo off
echo ===============================================================================
echo this is an in-offical build, the version will be determined on the SVN revision
echo.
echo.
echo.

set resultDir=..\results
if not exist %resultDir% mkdir %resultDir%

..\tools\nant\nant.exe -buildfile:nant.build -l:%resultDir%\Nant-Build.log integration-build
if ERRORLEVEL 1 goto Failed

..\tools\nant\nant.exe -buildfile:nant.build -l:%resultDir%\Nant-unit-tests.log unit-tests 
if ERRORLEVEL 1 goto Failed

..\tools\nant\nant.exe -buildfile:nant.build run-fxcop
if ERRORLEVEL 1 goto Failed

echo "compilation and unit testing completed. Log file and unit-tests results are stored in %resultDir%"
goto Success


:Failed
echo.
echo.
echo ===============================================================================
echo Failed
echo ===============================================================================
echo.
echo.
pause
goto End


:Success
echo.
echo.
echo ===============================================================================
echo Success
echo ===============================================================================
echo.
echo.
pause
goto End

:End