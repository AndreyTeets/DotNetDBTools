$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
Set-StrictMode -Version 3.0
Import-Module "$PSScriptRoot/common.psm1"

$repoRoot = "$PSScriptRoot/../.."
Push-Location $repoRoot
try {
    exec dotnet build DotNetDBTools.sln -c Debug
    exec dotnet pack --no-build DotNetDBTools.sln -c Debug

    New-Item -Path "./artifacts/nuget" -ItemType Directory -Force | Out-Null
    Copy-Item -Path "./NuGet.Config" -Destination "./artifacts/nuget/NuGet.Config" -Force
    exec dotnet nuget enable source "GeneratedDebugPackages" --configfile "./artifacts/nuget/NuGet.Config"
    exec dotnet nuget locals --clear global-packages

    exec dotnet build DotNetDBTools.sln -c Release "-p:UsePackagesInSampleApplications=True" --configfile "./artifacts/nuget/NuGet.Config"
    exec dotnet pack --no-build DotNetDBTools.sln -c Release
} finally {
    Pop-Location
}
