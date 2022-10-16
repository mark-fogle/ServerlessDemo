$resourceGroupName = $args[0]
$projectPrefix = $args[1]

& $PSScriptRoot/../src/Api/deploy/deploy.ps1 $resourceGroupName $projectPrefix
& $PSScriptRoot/../src/Web/deploy/deploy.ps1 $projectPrefix