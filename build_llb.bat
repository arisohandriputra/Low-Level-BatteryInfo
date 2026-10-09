@echo off
setlocal

windres llb.rc -O coff -o llb_resource.o
if errorlevel 1 goto failed

gcc llb.c llb_resource.o -o llb.exe -lsetupapi
if errorlevel 1 goto failed

del llb_resource.o 2>nul
echo.
echo Build completed: llb.exe
echo File version: 1.1.0.0
echo.
pause
exit /b 0

:failed
echo.
echo Build failed. Check the error messages above.
echo.
pause
exit /b 1
