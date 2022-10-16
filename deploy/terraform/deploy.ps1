$resourceGroupName = $args[0]
$projectPrefix = $args[1]

terraform init

if($LASTEXITCODE -ne 0) {
    throw "Error occurred durring terraform init" 
}

terraform validate

if($LASTEXITCODE -ne 0) {
    throw "Error occurred durring terraform validate" 
}

terraform apply -var="project-prefix=$resourceGroupName"

if($LASTEXITCODE -ne 0) {
    throw "Error occurred durring terraform apply" 
}

terraform output -raw web-app-settings > ..\..\src\Web\wwwroot\appsettings.json

..\deploy.ps1 $resourceGroupName $projectPrefix

terraform output -raw web-app-address 