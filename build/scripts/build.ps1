$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
dotnet build DotNetDBTools.sln -c Release
Pop-Location