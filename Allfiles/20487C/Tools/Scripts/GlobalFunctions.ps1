$global:initials=$null
$global:region=$null

Function Get-Region
{
    if ($global:region -eq $null)
    {
        $regions = Get-AzureLocation | select -ExpandProperty Name
        $global:region = Select-Item -Caption "Windows Azure Regions" -Message "Enter the region number closest to your location " -choice $regions
    }
    return $global:region
}

Function Get-Initials
{
    if ($global:initials -eq $null)
    {
        Write-Host "Windows Azure components are required to have unique names."
        Write-Host "To create a unique name, please enter your initials"
        Write-Host "For example, if your name is Mark Hanson, type MarkH"
        $global:initials = Read-Host -Prompt "Enter your initials"
    }
    return $global:initials
}

Function Select-Item 
{   
Param(  [String[]]$choiceList,                   
        [String]$Caption,
        [String]$Message        
) 
   Write-Host $Caption
   $index = 1;
   foreach($choice in $choiceList)
   {
        Write-Host "($index) $choice"
        $index++
   }

   $selection = [int](Read-Host -Prompt $Message)
   if ($selection -gt 0 -and $selection -lt $index)
   {
        return $choiceList[$selection-1]
   }
   else
   {
        Write-Error "Invalid Value"
   }  
}  

Function Update-CompanionWebConfig
{
Param(
    [Parameter(Mandatory=$true)]
    [ValidateScript({Test-Path $_ -PathType 'Leaf'})]
    [string] $sourceConfigPath,
    [Parameter(Mandatory=$true)]    
    [string] $targetConfigPath,
    [Parameter(Mandatory=$true)]    
    [string] $serviceBusNamespace,
    [Parameter(Mandatory=$true)]    
    [string] $serviceBusSecret,
    [Parameter(Mandatory=$true)]    
    [string] $dbServerName

)

    $file = get-content $sourceConfigPath
    $file = $file -replace '{ServiceBusNamespace}',$serviceBusNamespace
    $file = $file -replace '{AccessKey}',$serviceBusSecret
    $file = $file -replace '{DbServerName}',$dbServerName
            
    $file | set-content $targetConfigPath
}

Function Update-ServerWebConfig
{
Param(
    [Parameter(Mandatory=$true)]
    [ValidateScript({Test-Path $_ -PathType 'Leaf'})]
    [string] $sourceConfigPath,
    [Parameter(Mandatory=$true)]    
    [string] $targetConfigPath,
    [Parameter(Mandatory=$true)]    
    [string] $serviceBusNamespace,
    [Parameter(Mandatory=$true)]    
    [string] $serviceBusSecret

)

    $file = get-content $sourceConfigPath
    $file = $file -replace '{ServiceBusNamespace}',$serviceBusNamespace
    $file = $file -replace '{AccessKey}',$serviceBusSecret    
            
    $file | set-content $targetConfigPath
}

Function Update-ClientConfiguration
{
Param(
    [Parameter(Mandatory=$true)]
    [ValidateScript({Test-Path $_ -PathType 'Leaf'})]
    [string] $sourceConfigPath,
    [Parameter(Mandatory=$true)]    
    [string] $targetConfigPath,
    [Parameter(Mandatory=$true)]    
    [string] $cloudService

)

    $file = get-content $sourceConfigPath
    $file = $file -replace '{CloudService}',$cloudService
            
    $file | set-content $targetConfigPath
}
