@echo off
setlocal

powershell -NoProfile -ExecutionPolicy Bypass -Command "./build/scripts/build.ps1"

echo Press any key to continue...
pause >nul