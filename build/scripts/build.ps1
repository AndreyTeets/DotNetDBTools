$ErrorActionPreference = 'Stop'
$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
New-Item -Path "./artifacts/nuget/Debug" -ItemType Directory -Force | Out-Null

# Build analyzer-type projects first separately before sample projects
dotnet build "./src/DotNetDBTools.DefinitionAnalyzer" -c Debug
if ($LastExitCode -ne 0) {
    throw
}
dotnet build "./src/DotNetDBTools.DescriptionSourceGenerator" -c Debug
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

dotnet build DotNetDBTools.sln -c Release -p:UsePackagesInSampleApplications=True
if ($LastExitCode -ne 0) {
    throw
}
dotnet pack --no-build DotNetDBTools.sln -c Release
if ($LastExitCode -ne 0) {
    throw
}
Pop-Location