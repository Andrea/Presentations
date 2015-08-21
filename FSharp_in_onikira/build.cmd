@echo off
cls

rem .paket\paket.bootstrapper.exe prerelease
if errorlevel 1 (
  exit /b %errorlevel%
)

rem .paket\paket.exe restore -v
if errorlevel 1 (
  exit /b %errorlevel%
)

packages\FAKE\tools\FAKE.exe build.fsx %*
