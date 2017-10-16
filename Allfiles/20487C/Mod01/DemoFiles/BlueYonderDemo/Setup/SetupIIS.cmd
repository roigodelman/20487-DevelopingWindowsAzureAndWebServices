@Echo off

set EnableNuGetPackageRestore=True

"%systemroot%\system32\inetsrv\appcmd.exe" set apppool DefaultAppPool /startMode:AlwaysRunning /processModel.LoadUserProfile:true

%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe D:\Allfiles\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Server\BlueYonder.Companion.sln /noconsolelogger /nologo /m
"%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BlueYonder.Companion.Host"
"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/BlueYonder.Companion.Host /physicalPath:"D:\Allfiles\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Server\BlueYonder.Companion.Host"
"%systemroot%\system32\inetsrv\appcmd.exe" set app "Default Web Site/BlueYonder.Companion.Host" /preloadEnabled:true

%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe D:\Allfiles\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Server\BlueYonder.Server.sln /noconsolelogger /nologo /m

"%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BlueYonder.Server.Booking.WebHost"
"%systemroot%\system32\inetsrv\AppCmd.exe" delete app "Default Web Site/BlueYonder.Server.FrequentFlyer.WebHost"

"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/BlueYonder.Server.Booking.WebHost /physicalPath:"D:\Allfiles\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Server\BlueYonder.Server.Booking.WebHost"
"%systemroot%\system32\inetsrv\AppCmd.exe" add app /site.name:"Default Web Site" /path:/BlueYonder.Server.FrequentFlyer.WebHost /physicalPath:"D:\Allfiles\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Server\BlueYonder.Server.FrequentFlyer.WebHost"

"%systemroot%\system32\inetsrv\appcmd.exe" set app "Default Web Site/BlueYonder.Server.Booking.WebHost" /enabledProtocols:http,net.tcp
"%systemroot%\system32\inetsrv\appcmd.exe" set app "Default Web Site/BlueYonder.Server.FrequentFlyer.WebHost" /enabledProtocols:http,net.tcp

"%systemroot%\system32\inetsrv\AppCmd.exe" recycle apppool defaultapppool