[CmdletBinding(PositionalBinding = $false)]
param(
    [string]$NugetApiKey
)
$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
Set-StrictMode -Version 3.0
Import-Module "$PSScriptRoot/common.psm1"

$repoRoot = "$PSScriptRoot/../.."
Push-Location $repoRoot
try {
    #exec dotnet nuget push artifacts/nuget/Release/*.nupkg -s https://api.nuget.org/v3/index.json --api-key $NugetApiKey
    Write-Host "Echo: dotnet nuget push stub"
    exec dotnet --list-sdks
    Get-ChildItem artifacts/nuget/Release/
} finally {
    Pop-Location
}
