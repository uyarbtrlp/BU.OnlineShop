$slnFolder = $currentFolder = $PSScriptRoot

# Certificate creation
$etcFolder = Join-Path $currentFolder "etc"
$certsFolder = Join-Path $etcFolder "certs"

If(!(Test-Path -Path $certsFolder))
{
    New-Item -ItemType Directory -Force -Path $certsFolder
    
	if(!(Test-Path -Path (Join-Path $certsFolder "localhost.pfx") -PathType Leaf)){
        Set-Location $certsFolder
		Write-Host "********* Creating PFX Hosting Certificate *********" -ForegroundColor Magenta
        dotnet dev-certs https -v -ep localhost.pfx -p CBD4E783-BE68-4DFE-93DF-D625BC943A7A -t        
    }
	
	if(!(Test-Path -Path (Join-Path $certsFolder "localhost.pem") -PathType Leaf) -or !(Test-Path -Path (Join-Path $certsFolder "localhost.crt") -PathType Leaf)){
        Set-Location $certsFolder
		Write-Host "********* Creating CRT/PEM Hosting Certificate *********" -ForegroundColor Magenta     
		dotnet dev-certs https -v -ep localhost.crt -np --format pem -t
    }
	
	Set-Location $slnFolder
}

# Delete existing docker images
$containerIds = docker ps -a -q
Write-Host "********* CLEANING Existing Images *********" -ForegroundColor Green
foreach($containerId in $containerIds ) {
	$imageName = docker inspect $containerId --format "{{.Config.Image}}"
	if($imageName.Contains("onlineshop")) {
		Write-Host -ForegroundColor Yellow "Stopping Container : " + $containerId
		docker stop $containerId 
		Write-Host -ForegroundColor Magenta "Removing Container : " + $containerId
		docker rm $containerId 
	}
}

$imageIds = docker images -q -f "reference=onlineshop*/*"
foreach($imageId in $imageIds ) {
	Write-Host -ForegroundColor Green "Removing Image" 
	docker rmi $imageId
}

# Build docker images locally
$version = 'latest'

$webFolder = Join-Path $slnFolder "services/identity/BU.OnlineShop.Identity"
Write-Host "********* BUILDING Web Application *********" -ForegroundColor Green
Set-Location $webFolder
dotnet publish -c Release
docker build -f Dockerfile.Local -t onlineshop.identityservice:$version .

$webFolder = Join-Path $slnFolder "services/basket/BU.OnlineShop.BasketService.API"
Write-Host "********* BUILDING Web Application *********" -ForegroundColor Green
Set-Location $webFolder
dotnet publish -c Release
docker build -f Dockerfile.Local -t onlineshop.basketservice:$version .

$webFolder = Join-Path $slnFolder "services/catalog/BU.OnlineShop.CatalogService.API"
Write-Host "********* BUILDING Web Application *********" -ForegroundColor Green
Set-Location $webFolder
dotnet publish -c Release
docker build -f Dockerfile.Local -t onlineshop.catalogservice:$version .

$webFolder = Join-Path $slnFolder "services/file/BU.OnlineShop.FileService.API"
Write-Host "********* BUILDING Web Application *********" -ForegroundColor Green
Set-Location $webFolder
dotnet publish -c Release
docker build -f Dockerfile.Local -t onlineshop.fileservice:$version .

$webFolder = Join-Path $slnFolder "services/order/BU.OnlineShop.OrderingService.API"
Write-Host "********* BUILDING Web Application *********" -ForegroundColor Green
Set-Location $webFolder
dotnet publish -c Release
docker build -f Dockerfile.Local -t onlineshop.orderservice:$version .

$webFolder = Join-Path $slnFolder "services/payment/BU.OnlineShop.PaymentService.API"
Write-Host "********* BUILDING Web Application *********" -ForegroundColor Green
Set-Location $webFolder
dotnet publish -c Release
docker build -f Dockerfile.Local -t onlineshop.paymentservice:$version .

$webFolder = Join-Path $slnFolder "gateways/BU.OnlineShop.WebGateway"
Write-Host "********* BUILDING Web Application *********" -ForegroundColor Green
Set-Location $webFolder
dotnet publish -c Release
docker build -f Dockerfile.Local -t onlineshop.webgateway:$version .


Set-Location $slnFolder
Write-Host "********* Composing Docker *********" -ForegroundColor Green
docker-compose -f docker-compose.yml -f docker-compose.override.yml -f docker-compose.infrastructure.yml -f docker-compose.infrastructure.override.yml up -d # conmpose file using Docker.Local files