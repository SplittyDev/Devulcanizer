@echo off
@setlocal enableextensions
@cd /d "%~dp0"
net session >nul 2>&1
if %errorlevel% == 0 (
goto service_start
) else (
goto insufficient_permissions
)
:service_start
echo Starting service...
net start devulcanizer
goto end
:insufficient_permissions
echo Please run this file as administrator.
:end
pause