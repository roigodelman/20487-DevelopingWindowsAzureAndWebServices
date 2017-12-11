@Echo off
%~d0
cd %~dp0

@Echo Removing the client certificate if one was installed (will fail if this is the first time)
del /q Client.pfx > NUL
del /q Client.cer > NUL
"certmgr.exe" -del -c -n "Client" -s -r LocalMachine My

@Echo Creating a client certificate
"makecert.exe" -n "CN=Client" -pe -ss my -sr LocalMachine -sky exchange -in "Blue Yonder Airlines Root CA" -is my -ir LocalMachine Client.cer
certutil -privatekey -exportpfx -p 1 "Client" Client.pfx > NUL

powershell -NonInteractive -Command "Set-ExecutionPolicy unrestricted"
Powershell ..\..\..\tools\scripts\GrantCertsPermissions.ps1
pause