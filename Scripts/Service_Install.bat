@echo off
@setlocal enableextensions
@cd /d "%~dp0"
set installutil=C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe
net session >nul 2>&1
if %errorlevel% == 0 (
goto service_install
) else (
goto insufficient_permissions
)
:service_install
echo Installing service...
"%installutil%" "Devulcanizer.exe" >nul
if %errorlevel% == 0 (
echo Successfully installed Devulcanizer!
) else (
echo Installation failed.
)
goto end
:insufficient_permissions
echo Please run this file as administrator.
:end
pause