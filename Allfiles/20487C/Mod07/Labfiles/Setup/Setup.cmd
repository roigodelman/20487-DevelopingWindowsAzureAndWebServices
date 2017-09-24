@Echo off

%~d0
cd %~dp0

powershell -NonInteractive -Command "Set-ExecutionPolicy unrestricted"

@Echo Configuring machine's certificates
certmgr -del -c -n "Client" -s -r LocalMachine My 
certmgr -del -c -n "Server" -s -r LocalMachine My 
certmgr -del -c -n "Server" -s -r LocalMachine TrustedPeople 
certmgr -del -c -n "Blue Yonder Airlines Root CA" -s -r LocalMachine My 
certmgr -del -c -n "Blue Yonder Airlines Root CA" -s -r LocalMachine Root 

certutil -f -p 1 -importpfx ..\..\..\certs\BlueYonderAirlinesRootCA.pfx 
certutil -f -p 1 -importpfx ..\..\..\certs\client.pfx 
certutil -f -p 1 -importpfx ..\..\..\certs\server.pfx 
certutil -f -p 1 -importpfx ..\..\..\certs\cloudapp.pfx 
certutil -f -p 1 -importpfx ..\..\..\certs\emulator.pfx 
certutil -f -addstore trustedPeople ..\..\..\certs\server.cer 

Powershell ..\..\..\tools\scripts\GrantCertsPermissions.ps1
Powershell ..\..\..\tools\scripts\VerifyIIS_SSL.ps1

@Echo Checking for missing Windows Azure components
Powershell .\BuildWindowsAzureComponents.ps1

Rem Configuring IIS
Call ..\..\..\tools\scripts\SetupIIS.cmd Mod07 > NUL

pause