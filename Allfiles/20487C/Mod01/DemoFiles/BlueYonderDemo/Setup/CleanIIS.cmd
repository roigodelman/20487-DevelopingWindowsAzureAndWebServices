@Echo off

"%systemroot%\system32\inetsrv\appcmd.exe" set apppool DefaultAppPool /startMode:OnDemand

"%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BlueYonder.Companion.Host"
"%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BlueYonder.Server.Booking.WebHost"
"%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BlueYonder.Server.FrequentFlyer.WebHost"

"%systemroot%\system32\inetsrv\AppCmd.exe" recycle apppool defaultapppool