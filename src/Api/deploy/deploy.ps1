function ZipDeploy-AzureFunction([string]$projectPath, [string]$resourceGroupName, [string]$functionAppName ){
    
     az webapp config appsettings set -g $resourceGroupName -n $functionAppName --settings WEBSITE_RUN_FROM_PACKAGE=1

     dotnet clean -c Release $projectPath
     dotnet publish -c Release $projectPath
     $publishZip = "$($functionAppName).zip"
     $publishFolder = "$($projectPath)/bin/Release/net6.0/publish/*"

     if(Test-path $publishZip) {Remove-item $publishZip}
    Compress-Archive -Path $publishFolder -DestinationPath $publishZip -CompressionLevel Optimal

    az functionapp deployment source config-zip `
        -g $resourceGroupName -n $functionAppName --src $publishZip

	Remove-Item $publishZip
}

$resourceGroupName = $args[0]
$projectPrefix = $args[1]

dotnet restore $PSScriptRoot/../Api.csproj

#Deploy Api
ZipDeploy-AzureFunction -projectPath "$($PSScriptRoot)/.." `
    -resourceGroupName $resourceGroupName -functionAppName "$($projectPrefix)-api" -publishFolder "$($PSScriptRoot)/../bin/Release/net6.0/publish/*"