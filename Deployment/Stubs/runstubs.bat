@echo off

rem  Running a .bat file as administrator - Correcting current directory 
@setlocal enableextensions
@cd /d "%~dp0"

powershell .\runstubs.ps1

pause