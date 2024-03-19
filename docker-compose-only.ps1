$slnFolder = $currentFolder = $PSScriptRoot

Set-Location $slnFolder
Write-Host "********* Composing Docker *********" -ForegroundColor Green
docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.infrastructure.yml -f docker-compose.infrastructure.override.yml up -d # compose file using Docker.Local files