#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["services/file/BU.OnlineShop.FileService.API/BU.OnlineShop.FileService.API.csproj", "services/file/BU.OnlineShop.FileService.API/"]
COPY ["services/file/BU.OnlineShop.FileService.Database/BU.OnlineShop.FileService.Database.csproj", "services/file/BU.OnlineShop.FileService.Database/"]
COPY ["services/file/BU.OnlineShop.FileService.Domain/BU.OnlineShop.FileService.Domain.csproj", "services/file/BU.OnlineShop.FileService.Domain/"]
COPY ["services/file/BU.OnlineShop.FileService.Domain.Shared/BU.OnlineShop.FileService.Domain.Shared.csproj", "services/file/BU.OnlineShop.FileService.Domain.Shared/"]
COPY ["shared/BU.OnlineShop.Shared/BU.OnlineShop.Shared.csproj", "shared/BU.OnlineShop.Shared/"]
RUN dotnet restore "./services/file/BU.OnlineShop.FileService.API/BU.OnlineShop.FileService.API.csproj"
COPY . .
WORKDIR "/src/services/file/BU.OnlineShop.FileService.API"
RUN dotnet build "./BU.OnlineShop.FileService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BU.OnlineShop.FileService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BU.OnlineShop.FileService.API.dll"]