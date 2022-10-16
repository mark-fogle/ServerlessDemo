$projectPrefix = $args[0]

$projectPath = "$PSScriptRoot/../Web.csproj"
$publishPath = "$PSScriptRoot/../bin/Release/net6.0/publish/wwwroot"

dotnet clean -c Release $projectPath
dotnet restore $projectPath
dotnet publish -c Release $projectPath
az storage azcopy blob upload -c `$web --account-name "$($projectPrefix)storage" -s "$publishPath/*" --recursive
