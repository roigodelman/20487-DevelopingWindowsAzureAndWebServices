@echo off

@Echo Deleting certificate if already exists
"certmgr.exe" -del -c -n "DemoCert" -s -r LocalMachine My
@Echo Creating certificate for the demo
"makecert.exe" -n "CN=DemoCert" -pe -ss my -sr LocalMachine -sky exchange 

IF EXIST %WINDIR%\SysWow64 (
SET powerShellDir=%WINDIR%\SysWow64\windowspowershell\v1.0
) ELSE (
SET powerShellDir=%WINDIR%\system32\windowspowershell\v1.0
)

@echo Registering demo certificate for port 8081
%powerShellDir%\powershell.exe -NonInteractive -Command "Set-ExecutionPolicy unrestricted"
%powerShellDir%\powershell.exe -NonInteractive -Command ".\RegisterCert.ps1"

pause