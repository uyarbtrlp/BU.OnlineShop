#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/catalog/BU.OnlineShop.CatalogService.API/BU.OnlineShop.CatalogService.API.csproj", "services/catalog/BU.OnlineShop.CatalogService.API/"]
COPY ["integration/BU.OnlineShop.Integration.MessageBus/BU.OnlineShop.Integration.MessageBus.csproj", "integration/BU.OnlineShop.Integration.MessageBus/"]
COPY ["integration/BU.OnlineShop.Integration.Messages/BU.OnlineShop.Integration.Messages.csproj", "integration/BU.OnlineShop.Integration.Messages/"]
COPY ["services/basket/BU.OnlineShop.BasketService.Abstractions/BU.OnlineShop.BasketService.Abstractions.csproj", "services/basket/BU.OnlineShop.BasketService.Abstractions/"]
COPY ["services/catalog/BU.OnlineShop.CatalogService.Database/BU.OnlineShop.CatalogService.Database.csproj", "services/catalog/BU.OnlineShop.CatalogService.Database/"]
COPY ["services/catalog/BU.OnlineShop.CatalogService.Domain/BU.OnlineShop.CatalogService.Domain.csproj", "services/catalog/BU.OnlineShop.CatalogService.Domain/"]
COPY ["services/catalog/BU.OnlineShop.CatalogService.Domain.Shared/BU.OnlineShop.CatalogService.Domain.Shared.csproj", "services/catalog/BU.OnlineShop.CatalogService.Domain.Shared/"]
COPY ["shared/BU.OnlineShop.Shared/BU.OnlineShop.Shared.csproj", "shared/BU.OnlineShop.Shared/"]
RUN dotnet restore "services/catalog/BU.OnlineShop.CatalogService.API/BU.OnlineShop.CatalogService.API.csproj"
COPY . .
WORKDIR "/src/services/catalog/BU.OnlineShop.CatalogService.API"
RUN dotnet build "BU.OnlineShop.CatalogService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BU.OnlineShop.CatalogService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BU.OnlineShop.CatalogService.API.dll"]