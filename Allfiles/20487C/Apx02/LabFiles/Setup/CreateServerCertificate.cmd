@Echo off


@Echo Removing previously installed certificates (will fail if this is the first time)
del /q BlueYonderAirlinesRootCA.cer > NUL
del /q BlueYonderAirlinesRootCA.pfx > NUL
del /q Server.pfx > NUL
"certmgr.exe" -del -c -n "Server" -s -r LocalMachine My > NUL
"certmgr.exe" -del -c -n "Server" -s -r LocalMachine TrustedPeople > NUL
"certmgr.exe" -del -c -n "Blue Yonder Airlines Root CA" -s -r LocalMachine My > NUL
"certmgr.exe" -del -c -n "Blue Yonder Airlines Root CA" -s -r LocalMachine Root > NUL

Echo Creating a Root CA certificate
"makecert.exe" -n "CN=Blue Yonder Airlines Root CA" -pe -ss my -sr LocalMachine -sky exchange -r BlueYonderAirlinesRootCA.cer
certutil -f -addstore root BlueYonderAirlinesRootCA.cer > NUL
certutil -privatekey -exportpfx -p 1 "Blue Yonder Airlines Root CA" BlueYonderAirlinesRootCA.pfx > NUL

Echo Creating a server certificate
"makecert.exe" -n "CN=Server" -pe -ss my -sr LocalMachine -sky exchange -in "Blue Yonder Airlines Root CA" -is my -ir LocalMachine Server.cer
certutil -privatekey -exportpfx -p 1 "Server" Server.pfx > NUL
certutil -f -addstore trustedpeople Server.cer > NUL

pause