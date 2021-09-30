param(
    [string]$nugetApiKey
)

$ErrorActionPreference = 'Stop'
$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
Write-Host "Echo: dotnet nuget push"
dotnet --list-sdks
Get-ChildItem artifacts/nuget/Release/
#dotnet nuget push artifacts/nuget/Release/*.nupkg -s https://api.nuget.org/v3/index.json --api-key $nugetApiKey
if ($LastExitCode -ne 0) {
    throw
}
Pop-Location