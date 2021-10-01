$ErrorActionPreference = 'Stop'
$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot

dotnet build DotNetDBTools.sln -c Debug
if ($LastExitCode -ne 0) {
    throw
}
dotnet pack --no-build DotNetDBTools.sln -c Debug
if ($LastExitCode -ne 0) {
    throw
}

New-Item -Path "./artifacts/nuget" -ItemType Directory -Force | Out-Null
Copy-Item -Path "./NuGet.Config" -Destination "./artifacts/nuget/NuGet.Config" -Force
dotnet nuget enable source "GeneratedDebugPackages" --configfile "./artifacts/nuget/NuGet.Config"
dotnet nuget locals --clear global-packages

dotnet build DotNetDBTools.sln -c Release -p:UsePackagesInSampleApplications=True --configfile "./artifacts/nuget/NuGet.Config"
if ($LastExitCode -ne 0) {
    throw
}
dotnet pack --no-build DotNetDBTools.sln -c Release
if ($LastExitCode -ne 0) {
    throw
}

Pop-Location