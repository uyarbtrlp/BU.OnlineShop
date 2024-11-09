#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/catalog/BU.OnlineShop.CatalogService.API/BU.OnlineShop.CatalogService.API.csproj", "services/catalog/BU.OnlineShop.CatalogService.API/"]
COPY ["services/catalog/BU.OnlineShop.CatalogService.Database/BU.OnlineShop.CatalogService.Database.csproj", "services/catalog/BU.OnlineShop.CatalogService.Database/"]
COPY ["services/catalog/BU.OnlineShop.CatalogService.Domain/BU.OnlineShop.CatalogService.Domain.csproj", "services/catalog/BU.OnlineShop.CatalogService.Domain/"]
COPY ["services/catalog/BU.OnlineShop.CatalogService.Domain.Shared/BU.OnlineShop.CatalogService.Domain.Shared.csproj", "services/catalog/BU.OnlineShop.CatalogService.Domain.Shared/"]
COPY ["shared/BU.OnlineShop.Shared/BU.OnlineShop.Shared.csproj", "shared/BU.OnlineShop.Shared/"]
COPY ["services/basket/BU.OnlineShop.BasketService.Abstractions/BU.OnlineShop.BasketService.Abstractions.csproj", "services/basket/BU.OnlineShop.BasketService.Abstractions/"]
RUN dotnet restore "./services/catalog/BU.OnlineShop.CatalogService.API/BU.OnlineShop.CatalogService.API.csproj"
COPY . .
WORKDIR "/src/services/catalog/BU.OnlineShop.CatalogService.API"
RUN dotnet build "./BU.OnlineShop.CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BU.OnlineShop.CatalogService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BU.OnlineShop.CatalogService.API.dll"]