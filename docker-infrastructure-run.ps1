$slnFolder = $currentFolder = $PSScriptRoot

Set-Location $slnFolder
Write-Host "********* Starting Infrastructure Stack *********" -ForegroundColor Green
docker compose -f docker-compose.infrastructure.yml -f docker-compose.infrastructure.override.yml up -d