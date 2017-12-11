Function Get-DatabaseServerName
{
Param(
    [Parameter(Mandatory=$true)]    
    [bool] $removeExistingDbs = $true
)
    $adminName = "BlueYonderAdmin"
    $adminPwd = 'Pa$$w0rd'

    $secpasswd = ConvertTo-SecureString $adminPwd -AsPlainText -Force
    $dbcreds = New-Object System.Management.Automation.PSCredential ($adminName, $secpasswd)
        
    $dbServers = Get-AzureSqlDatabaseServer | where {$_.AdministratorLogin -eq $adminName}

    if ($dbServers.length -eq 0)
    {        
        # Create the database server and set firewall rules
        $region = Get-Region
        Write-Host Creating SQL Database server, please wait... -ForegroundColor Cyan
        Write-Host 
        $dbServer = New-AzureSqlDatabaseServer -AdministratorLogin $adminName -AdministratorLoginPassword $adminPwd -Location $region -Version 12.0
        New-AzureSqlDatabaseServerFirewallRule -ServerName $dbServer.ServerName -RuleName "OpenAllIps" -EndIpAddress "255.255.255.255" -StartIpAddress "0.0.0.0" | Out-Null
        return $dbServer.ServerName
    }
    else
    {
        # Server exists - remove old BlueYonder databases
        foreach($dbServer in $dbServers)
        {
            $ctx = New-AzureSqlDatabaseServerContext -Credential $dbcreds -ServerName $dbServer.ServerName -WarningAction Ignore
            # Delete previous BlueYonder databases
            if ($removeExistingDbs)
            {
                Get-AzureSqlDatabase $ctx | where {$_.Name -like 'BlueYonder*'} | Remove-AzureSqlDatabase -Force          
            }
        }
        return $dbServers[0].ServerName
    }        
}
