#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
Set-StrictMode -Version 3.0
Import-Module "$PSScriptRoot/common.psm1"

function ClearDotNetDBToolsNugetPackagesGlobalCache {
    if ($Env:CI) {
        exec dotnet nuget locals --clear global-packages
    } else {
        Write-Host "Clearing DotNetDBTools nuget packages global cache"
        Get-ChildItem -Path "~/.nuget/packages" -Filter "DotNetDBTools.*" -Force | Remove-Item -Recurse -Force
    }
}

$repoRoot = "$PSScriptRoot/../.."
Push-Location $repoRoot
try {
    ClearDotNetDBToolsNugetPackagesGlobalCache
    New-Item -Path "./artifacts/nuget" -ItemType Directory -Force | Out-Null
    Copy-Item -Path "./NuGet.Config" -Destination "./artifacts/nuget/NuGet.Config" -Force
    exec dotnet nuget enable source "GeneratedDebugPackages" --configfile "./artifacts/nuget/NuGet.Config"

    exec dotnet build-server shutdown
    exec dotnet build DotNetDBTools.sln -c Debug
    exec dotnet pack --no-build DotNetDBTools.sln -c Debug

    exec dotnet build-server shutdown
    exec dotnet build DotNetDBTools.sln -c Release --configfile "./artifacts/nuget/NuGet.Config" "-p:UsePackagesInSampleApplications=true" "-p:CI_BUILD=true"
    exec dotnet pack --no-build DotNetDBTools.sln -c Release
} finally {
    Pop-Location
}
