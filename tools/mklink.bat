@echo off
cd /d %~dp0
cd ..\build
gsudo mklink /D "x64\Debug\net8.0-windows10.0.19041.0\Plugins\WpfLibrary1" "..\..\..\..\Plugins\x64\Debug\net8.0-windows10.0.19041.0"
pause