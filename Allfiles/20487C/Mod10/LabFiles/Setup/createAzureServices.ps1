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
 $subscriptionId
)
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
$resourceGroupName = "BlueYonder.Lab.10"
$today = Get-Date -format yyyy-MM-dd;

# sign in
Write-Host "Logging in...";
Login-AzureRmAccount;

# select subscription
Write-Host "Selecting subscription '$subscriptionId'";
Select-AzureRmSubscription -SubscriptionID $subscriptionId;

# Register RPs
$resourceProviders = @("microsoft.sql");
if($resourceProviders.length) {
    Write-Host "Registering resource providers"
    foreach($resourceProvider in $resourceProviders) {
        RegisterRP($resourceProvider);
    }
}

#Create or check for existing resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
$resourceGroupLocation;
if(!$resourceGroup)
{
    Write-Host "Resource group '$resourceGroupName' does not exist. To create a new resource group, please enter a location.";
    $resourceGroupLocation = Read-Host "resourceGroupLocation";
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation
}
else{
    Write-Host "Using existing resource group '$resourceGroupName'";
	$resourceGroupLocation = $resourceGroup.Location;
}

# Get user's initials
Write-Host "Please enter your name's initials: (e.g. - John Doe = jd)";
$initials = Read-Host "Initials";
$serverName = "blueyonder10-$initials";
$databaseName = "BlueYonder.Companion.Lab10";
$hubNamespaceName = "blueyonder10-$initials";
$serviceBusNamespace = "blueyonder-10-$initials";
$serviceBusRelayNamespace = "blueyonder-10-relay-$initials";
$webappName = "blueyonder-companion-10-$initials";
$hubName = "blueyonder10Hub";
$password = 'Pa$$w0rd';
$storageAccountName = "blueyonder10$initials"

# Start the deployment
# Resource creation
Write-Host "Starting deployment of Azure SQL...";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile "./azureSql.json" -serverName $serverName -databaseName $databaseName;
Write-Host "Starting deployment of Azure Notification Hub...";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile "./notificationHub.json" -namespaceName $hubNamespaceName -notificationHubName $hubName;
Write-Host "Starting deployment of Azure Service Bus...";
New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile "./serviceBus.json" -serviceBusNamespaceName $serviceBusNamespace -SharedAccessKeyName "RootManageSharedAccessKey";
Write-Host "Starting deployment of Azure Relay...";
New-AzureRmRelayNamespace -Location $resourceGroupLocation -Name $serviceBusRelayNamespace -ResourceGroupName $resourceGroupName;
Write-Host "Starting deployment of Azure App Service..."
$appServicePlan = New-AzureRmAppServicePlan -Location $resourceGroupLocation -ResourceGroupName $resourceGroupName -Name $webappName -Tier "Free"
New-AzureRmWebApp -ResourceGroupName $resourceGroupName -Location $resourceGroupLocation -Name $webappName -AppServicePlan $appServicePlan.Name
Write-Host "Starting deployment of Azure Storage Account..."
New-AzureRmStorageAccount -ResourceGroupName $resourceGroupName -Location $resourceGroupLocation -Name $storageAccountName -SkuName Standard_LRS


# post-creation
$hubKeys = Get-AzureRmNotificationHubListKeys -AuthorizationRule DefaultFullSharedAccessSignature -ResourceGroup $resourceGroupName -Namespace blueyonder10-nhz -NotificationHub blueyonder10Hub
$dbConnectionString = "Server=tcp:$serverName.database.windows.net,1433;Initial Catalog=$databaseName;Persist Security Info=False;User ID=BlueYonderAdmin;Password=$password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=180;"
$hubConnectionString = "${hubKeys.PrimaryConnectionString}";
$relayKeyInfo = Get-AzureRmRelayKey -Namespace $serviceBusRelayNamespace -ResourceGroupName $resourceGroupName -Name RootManageSharedAccessKey
$storageKeys = Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName -Name $storageAccountName; 

Write-Host "Generating BlueYonder.Companion web.config...";
Write-Host "Existing web.config backed up at web.config.backup";
Get-Content -Path "../begin/BlueYonder.Server/BlueYonder.Companion.Host/web.config" | Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Companion.Host/web.config.backup"
$companionConfig = Get-Content "CompanionWeb.config";
$companionConfig = $companionConfig.replace("{azureDBConnection}", $dbConnectionString);
$companionConfig = $companionConfig.replace("{ServiceBusRelaySASKey}", $relayKeyInfo.PrimaryKey);
$companionConfig = $companionConfig.replace("{relayNamespace}", $serviceBusRelayNamespace);
$companionConfig = $companionConfig.replace("{storageConnectionString}", "DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageKeys[0].Value};EndpointSuffix=core.windows.net")
Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Companion.Host/web.config" -Value $companionConfig
Write-Host "Generating BlueYonder.Server web.config...";
Write-Host "Existing web.config backed up at web.config.backup";
Get-Content -Path "../begin/BlueYonder.Server/BlueYonder.Server.Booking.WebHost/web.config" | Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Server.Booking.WebHost/web.config.backup"
$serverConfig = Get-Content "ServerWeb.config";
$serverConfig = $serverConfig.replace("{ServiceBusRelaySASKey}", $relayKeyInfo.PrimaryKey);
$serverConfig = $serverConfig.replace("{relayNamespace}", $serviceBusRelayNamespace);
Set-Content -Path "../begin/BlueYonder.Server/BlueYonder.Server.Booking.WebHost/web.config" -Value $serverConfig
Write-Host "Please write down the following values:"
Write-Host "Relay namespace: $serviceBusRelayNamespace"
Write-Host "BlueYonder Companion App Service: http://$webappName.azurewebsites.com/"
Write-Host "Lab setup is done."
