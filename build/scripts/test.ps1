$repoRoot = "$PSScriptRoot/../.."

Push-Location $repoRoot
dotnet test DotNetDBTools.sln -c Release
Pop-Location