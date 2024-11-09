#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/basket/BU.OnlineShop.BasketService.API/BU.OnlineShop.BasketService.API.csproj", "services/basket/BU.OnlineShop.BasketService.API/"]
COPY ["services/basket/BU.OnlineShop.BasketService.Abstractions/BU.OnlineShop.BasketService.Abstractions.csproj", "services/basket/BU.OnlineShop.BasketService.Abstractions/"]
COPY ["services/basket/BU.OnlineShop.BasketService.Database/BU.OnlineShop.BasketService.Database.csproj", "services/basket/BU.OnlineShop.BasketService.Database/"]
COPY ["services/basket/BU.OnlineShop.BasketService.Domain/BU.OnlineShop.BasketService.Domain.csproj", "services/basket/BU.OnlineShop.BasketService.Domain/"]
COPY ["services/basket/Bu.OnlineShop.BasketService.Domain.Shared/Bu.OnlineShop.BasketService.Domain.Shared.csproj", "services/basket/Bu.OnlineShop.BasketService.Domain.Shared/"]
COPY ["shared/BU.OnlineShop.Shared/BU.OnlineShop.Shared.csproj", "shared/BU.OnlineShop.Shared/"]
RUN dotnet restore "./services/basket/BU.OnlineShop.BasketService.API/BU.OnlineShop.BasketService.API.csproj"
COPY . .
WORKDIR "/src/services/basket/BU.OnlineShop.BasketService.API"
RUN dotnet build "./BU.OnlineShop.BasketService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BU.OnlineShop.BasketService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BU.OnlineShop.BasketService.API.dll"]