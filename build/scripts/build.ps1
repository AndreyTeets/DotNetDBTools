$ErrorActionPreference = 'Stop'
$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
New-Item -Path "./artifacts/nuget/Debug" -ItemType Directory -Force | Out-Null

dotnet build DotNetDBTools.sln -c Debug
if ($LastExitCode -ne 0) {
    throw
}
dotnet pack --no-build DotNetDBTools.sln -c Debug
if ($LastExitCode -ne 0) {
    throw
}

dotnet nuget locals --clear global-packages
dotnet build DotNetDBTools.sln -c Release -p:UsePackagesInSampleApplications=True
if ($LastExitCode -ne 0) {
    throw
}
dotnet pack --no-build DotNetDBTools.sln -c Release
if ($LastExitCode -ne 0) {
    throw
}
Pop-Location