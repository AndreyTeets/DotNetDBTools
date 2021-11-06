$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
Set-StrictMode -Version 3.0
Import-Module "$PSScriptRoot/common.psm1"

$repoRoot = "$PSScriptRoot/../.."
Push-Location $repoRoot
try {
    exec dotnet test --no-build DotNetDBTools.sln -c Release
} finally {
    Pop-Location
}
