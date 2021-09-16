$ErrorActionPreference = 'Stop'
$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
New-Item -Path "./artifacts/nuget/Debug" -ItemType Directory -Force | Out-Null

dotnet build "./src/Tools/DotNetDBTools.DbDescriptionGenerator" -c Debug
if ($LastExitCode -ne 0) {
    throw
}

dotnet build DotNetDBTools.sln -c Debug
if ($LastExitCode -ne 0) {
    throw
}

dotnet pack --no-build DotNetDBTools.sln -c Debug
if ($LastExitCode -ne 0) {
    throw
}

dotnet build DotNetDBTools.sln -c Release
if ($LastExitCode -ne 0) {
    throw
}

dotnet pack --no-build DotNetDBTools.sln -c Release
if ($LastExitCode -ne 0) {
    throw
}
Pop-Location