$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"
Set-StrictMode -Version 3.0

function exec([string]$_cmd) {
    $datetime = [DateTime]::Now.ToString("yyyy-MM-dd HH:mm:ss")
    Write-Host -ForegroundColor DarkGray "$datetime >>> exec $_cmd $args"
    & $_cmd @args
    if ($LASTEXITCODE -ne 0) {
        throw "exec '$_cmd $args' failed with exit code $LASTEXITCODE"
    }
}
