$ErrorActionPreference = 'Stop'
$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
dotnet test --no-build DotNetDBTools.sln -c Release
if ($LastExitCode -ne 0) {
    throw
}
Pop-Location