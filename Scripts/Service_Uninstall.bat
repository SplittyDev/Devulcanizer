@echo off
@setlocal enableextensions
@cd /d "%~dp0"
set installutil=C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe
net session >nul 2>&1
if %errorlevel% == 0 (
goto service_uninstall
) else (
goto insufficient_permissions
)
:service_uninstall
echo Removing service...
"%installutil%" -U "Devulcanizer.exe" >nul
if %errorlevel% == 0 (
echo Successfully removed Devulcanizer!
) else (
echo Something went wrong.
)
goto end
:insufficient_permissions
echo Please run this file as administrator.
:end
pause