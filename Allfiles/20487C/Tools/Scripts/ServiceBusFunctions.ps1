Function Get-ServiceBusNamespace
{
    $props = @{
        Namespace = $sbNamespaceName
        SecretKey = $sbSecretKey    
    }

    $sbNamespace = new-object psobject -Property $props

    $sbNamespaces = Get-AzureSBNamespace
    $sbNamespaces = $sbNamespaces | where {$_.Name -like 'BlueYonderServerLab*'}
    $connStr = ""

    if ($sbNamespaces -eq $null -or $sbNamespaces.Length -eq 0)
    {
        $initials = Get-Initials

        $date = Get-Date
        $date = $date.ToString("yyyy-MM-dd")

        $newSbNamespaceName = 'BlueYonderServerLab-' + $date + "-" + $initials
        
        $region = Get-Region
        Write-Host Creating Service Bus namespace, please wait... -ForegroundColor Cyan
        Write-Host 

        $newSbNameSpace = New-AzureSBNamespace -Location $region -Name $newSbNamespaceName -NamespaceType Messaging -CreateACSNamespace $false
		while ($newSbNameSpace.Status -ne 'Active')
		{
		 sleep 5
		 $newSbNameSpace = Get-AzureSBNamespace $newSbNamespaceName
		}
        $sbNamespace.Namespace = $newSbNameSpace.Name
        #$sbNamespace.SecretKey = $newSbNameSpace.DefaultKey
        $connStr = $newSbNameSpace.ConnectionString         
    }
    else
    {
        $sbNamespace.Namespace = $sbNamespaces[0].Name
        #$sbNamespace.SecretKey = $sbNamespaces[0].DefaultKey
        $connStr = $sbNamespaces[0].ConnectionString         
    }
    
    $sbNamespace.SecretKey = $connStr.Substring($connStr.IndexOf("SharedAccessKey=")+"SharedAccessKey=".Length)
    return $sbNamespace;
}
