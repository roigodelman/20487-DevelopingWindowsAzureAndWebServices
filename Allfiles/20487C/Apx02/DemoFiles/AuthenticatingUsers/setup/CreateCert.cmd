@echo off

@Echo Deleting certificate if already exists
"certmgr.exe" -del -c -n "DemoCert" -s -r LocalMachine My
@Echo Creating certificate for the demo
"makecert.exe" -n "CN=DemoCert" -pe -ss my -sr LocalMachine -sky exchange 

pause