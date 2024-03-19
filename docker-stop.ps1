$slnFolder = $currentFolder = $PSScriptRoot

Set-Location $slnFolder
Write-Host "********* Stopping Stack *********" -ForegroundColor Green
docker compose down