<#
 .SYNOPSIS
    Deploys a template to Azure

 .DESCRIPTION
    Deploys an Azure Resource Manager template

 .PARAMETER subscriptionId
    The subscription id where the template will be deployed.
#>

param(
 [Parameter(Mandatory=$True)]
 [string]
 $subscriptionId)

<#
.SYNOPSIS
    Registers RPs
#>
Function RegisterRP {
    Param(
        [string]$ResourceProviderNamespace
    )

    Write-Host "Registering resource provider '$ResourceProviderNamespace'";
    Register-AzureRmResourceProvider -ProviderNamespace $ResourceProviderNamespace;
}

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
$ErrorActionPreference = "Stop"
$resourceGroupName = "BlueYonder.Lab.09"

# sign in
Write-Host "Logging in...";
try {
    Get-AzureRmContext
} catch {
    Login-AzureRmAccount;
}

# select subscription
Write-Host "Selecting subscription '$subscriptionId'";
Select-AzureRmSubscription -SubscriptionID $subscriptionId;

# Register RPs
$resourceProviders = @("microsoft.sql", "microsoft.relay");
if($resourceProviders.length) {
    Write-Host "Registering resource providers"
    foreach($resourceProvider in $resourceProviders) {
        RegisterRP($resourceProvider);
    }
}

#Create or check for existing resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if(!$resourceGroup)
{
    Write-Host "Resource group '$resourceGroupName' does not exist. To create a new resource group, please enter a location.";
    $resourceGroupLocation = Read-Host "resourceGroupLocation";
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation
}
else{
    Write-Host "Using existing resource group '$resourceGroupName'";
    $resourceGroupLocation = $resourceGroup[0].Location;
}

# Get user's initials
Write-Host "Please enter your name's initials: (e.g. - John Doe = jd)";
$initials = Read-Host "Initials";
$serverName = "blueyonder09-$initials";
$databaseName = "BlueYonder.Companion.Lab09";
$hubNamespaceName = "blueyonder09-$initials";
$hubName = "blueyonder09Hub";
$password = 'Pa$$w0rd';
$serviceBusRelayName = "blueyonder09Relay-$initials";

# Start the deployment
Write-Host "Starting deployment of Azure SQL...";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile "./azureSql.json" -serverName $serverName -databaseName $databaseName;
$response = Invoke-WebRequest ifconfig.me/ip
$clientIP = $response.Content.Trim()
New-AzureRmSqlServerFirewallRule -StartIpAddress $clientIP -EndIpAddress $clientIP -ResourceGroupName $resourceGroupName -ServerName $serverName -FirewallRuleName ClientIP
Write-Host "Starting deployment of Azure Notification Hub...";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile "./notificationHub.json" -namespaceName $hubNamespaceName -notificationHubName $hubName;
Write-Host "Starting deployment of Azure Relay...";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile "./relay.json" -nameFromTemplate $serviceBusRelayName -location $resourceGroupLocation;

$hubKeys = Get-AzureRmNotificationHubListKeys -AuthorizationRule DefaultFullSharedAccessSignature -ResourceGroup BlueYonder.Lab.07 -Namespace blueyonder09-nhz -NotificationHub blueyonder09Hub
$dbConnectionString = "Server=tcp:$serverName.database.windows.net,1433;Initial Catalog=$databaseName;Persist Security Info=False;User ID=BlueYonderAdmin;Password=$password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=180;"
$hubConnectionString = "${hubKeys.PrimaryConnectionString}";
$relayKeyInfo = Get-AzureRmRelayKey -Namespace $serviceBusRelayName -ResourceGroupName $resourceGroupName -Name RootManageSharedAccessKey


Write-Host "Generating BlueYonder.Companion web.config...";
Write-Host "Existing web.config backed up at web.config.backup";
Get-Content -Path "../begin/BlueYonder.Server/BlueYonder.Companion.Host/web.config" | Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Companion.Host/web.config.backup"
$companionConfig = Get-Content "CompanionWeb.config";
$companionConfig = $companionConfig.replace("{azureDBConnection}", $dbConnectionString);
$companionConfig = $companionConfig.replace("{ServiceBusRelaySASKey}", $relayKeyInfo.PrimaryKey);
$companionConfig = $companionConfig.replace("{relayNamespace}", $serviceBusRelayName);
Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Companion.Host/web.config" -Value $companionConfig
Write-Host "Generating BlueYonder.Server web.config...";
Write-Host "Existing web.config backed up at web.config.backup";
Get-Content -Path "../begin/BlueYonder.Server/BlueYonder.Server.Booking.WebHost/web.config" | Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Server.Booking.WebHost/web.config.backup"
$serverConfig = Get-Content "ServerWeb.config";
$serverConfig = $serverConfig.replace("{ServiceBusRelaySASKey}", $relayKeyInfo.PrimaryKey);
$serverConfig = $serverConfig.replace("{relayNamespace}", $serviceBusRelayName);
Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Server.Booking.WebHost/web.config" -Value $serverConfig
Write-Host "Lab setup is done."