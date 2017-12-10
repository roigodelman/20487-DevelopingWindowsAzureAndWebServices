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
$resourceGroupName = "BlueYonder.Lab.08"
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
$serverName = "blueyonder08-$initials";
$databaseName = "BlueYonder.Companion.Lab08";
$hubNamespaceName = "blueyonder08-$initials";
$serviceBusNamespace = "blueyonder-$today-$initials";
$serviceBusRelayNamespace = "blueyonder-$today-relay-$initials";
$hubName = "blueyonder08Hub";
$password = 'Pa$$w0rd';

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


# post-creation
$hubKeys = Get-AzureRmNotificationHubListKeys -AuthorizationRule "DefaultFullSharedAccessSignature" -Namespace $hubNamespaceName -NotificationHub $hubName -ResourceGroup $resourceGroupName
$servicebusKeys = Get-AzureRmServiceBusKey -Name "RootManageSharedAccessKey" -Namespace $serviceBusNamespace -ResourceGroup $resourceGroupName
$servicebusrelayKeys = Get-AzureRmRelayKey -Namespace $serviceBusRelayNamespace -ResourceGroupName $resourceGroupName -Name "RootManageSharedAccessKey"
Write-Host "Connection string for database '$databaseName :";
Write-Host "Server=tcp:$serverName.database.windows.net,1433;Initial Catalog=$databaseName;Persist Security Info=False;User ID=BlueYonderAdmin;Password=$password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=180;"
Write-Host "Connection string for notificiation hub '$hubName' :";
Write-Host $hubKeys.PrimaryConnectionString;
Write-Host "Connection string for Service Bus '$serviceBusNamespace' :";
Write-Host $servicebusKeys.PrimaryConnectionString;
Write-Host "Connection string for Service Bus Relay '$serviceBusRelayNamespace' :";
Write-Host $servicebusrelayKeys.PrimaryConnectionString;


