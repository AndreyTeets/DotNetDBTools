$ErrorActionPreference = 'Stop'
$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
dotnet test DotNetDBTools.sln -c Release
if ($LastExitCode -ne 0) {
    throw
}
Pop-Location