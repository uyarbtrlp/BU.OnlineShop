$slnFolder = $currentFolder = $PSScriptRoot

Set-Location $slnFolder
Write-Host "********* Stopping Infrastructure Stack *********" -ForegroundColor Green
docker compose -f docker-compose.infrastructure.yml -f docker-compose.infrastructure.override.yml down