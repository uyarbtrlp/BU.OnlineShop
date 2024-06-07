#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/order/BU.OnlineShop.OrderingService.API/BU.OnlineShop.OrderingService.API.csproj", "services/order/BU.OnlineShop.OrderingService.API/"]
COPY ["services/order/BU.OnlineShop.OrderingService.Database/BU.OnlineShop.OrderingService.Database.csproj", "services/order/BU.OnlineShop.OrderingService.Database/"]
COPY ["services/order/BU.OnlineShop.OrderingService.Domain/BU.OnlineShop.OrderingService.Domain.csproj", "services/order/BU.OnlineShop.OrderingService.Domain/"]
COPY ["integration/BU.OnlineShop.Integration.MessageBus/BU.OnlineShop.Integration.MessageBus.csproj", "integration/BU.OnlineShop.Integration.MessageBus/"]
COPY ["integration/BU.OnlineShop.Integration.Messages/BU.OnlineShop.Integration.Messages.csproj", "integration/BU.OnlineShop.Integration.Messages/"]
COPY ["shared/BU.OnlineShop.Shared/BU.OnlineShop.Shared.csproj", "shared/BU.OnlineShop.Shared/"]
COPY ["services/basket/BU.OnlineShop.BasketService.Abstractions/BU.OnlineShop.BasketService.Abstractions.csproj", "services/basket/BU.OnlineShop.BasketService.Abstractions/"]
COPY ["services/order/BU.OnlineShop.OrderingService.Domain.Shared/BU.OnlineShop.OrderingService.Domain.Shared.csproj", "services/order/BU.OnlineShop.OrderingService.Domain.Shared/"]
RUN dotnet restore "./services/order/BU.OnlineShop.OrderingService.API/BU.OnlineShop.OrderingService.API.csproj"
COPY . .
WORKDIR "/src/services/order/BU.OnlineShop.OrderingService.API"
RUN dotnet build "./BU.OnlineShop.OrderingService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BU.OnlineShop.OrderingService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BU.OnlineShop.OrderingService.API.dll"]