@echo off
setlocal
cd /d %~dp0

powershell -NoProfile -ExecutionPolicy Bypass -Command "./build/scripts/test.ps1"

echo Press any key to close this window...
pause >nul
