#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
Set-StrictMode -Version 3.0
Import-Module "$PSScriptRoot/common.psm1"

$testOptions = @(
    "--no-build"
    "-c"
    "Release"
    "-p:CollectCoverage=true"
    "-p:UseSourceLink=false"
    "-p:IncludeTestAssembly=false"
    "-p:SkipAutoProps=true"
    "-p:DeterministicReport=false"
    "-p:Exclude=[DotNetDBTools.Sample*]*%2c[DotNetDBTools.CodeParsing]DotNetDBTools.CodeParsing.Generated.*"
    "-p:CoverletOutput=../../tests/TestResults/"
    "-p:MergeWith=../../tests/TestResults/coverage.json"
)

$repoRoot = "$PSScriptRoot/../.."
Push-Location $repoRoot
try {
    if (Test-Path -Path "./tests/TestResults") {
        Remove-Item -Force -Recurse -Path "./tests/TestResults"
    }

    exec dotnet tool restore

    $env:RECREATE_CONTAINERS = "true"
    exec dotnet test "./tests/DotNetDBTools.UnitTests" @testOptions "-p:CoverletOutputFormat=json"
    exec dotnet test "./tests/DotNetDBTools.AnalyzersTests" @testOptions "-p:CoverletOutputFormat=json"
    exec dotnet test "./tests/DotNetDBTools.IntegrationTests" @testOptions "-p:CoverletOutputFormat=opencover"

    exec dotnet reportgenerator "-reports:./tests/TestResults/coverage.opencover.xml" "-targetdir:./tests/TestResults/report"
} finally {
    Pop-Location
}
