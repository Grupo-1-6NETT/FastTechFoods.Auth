# FASTTECHFOODS.AUTH
# BASE
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# BUILD
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Auth.API/Auth.API.csproj", "src/Auth.API/"]
COPY ["src/Auth.Application/Auth.Application.csproj", "src/Auth.Application/"]
COPY ["src/Auth.Core/Auth.Core.csproj", "src/Auth.Core/"]
COPY ["src/Auth.Exception/Auth.Exception.csproj", "src/Auth.Exception/"]
COPY ["src/Auth.Infrastructure/Auth.Infrastructure.csproj", "src/Auth.Infrastructure/"]
RUN dotnet restore "./src/Auth.API/Auth.API.csproj"
COPY . .
WORKDIR "/src/src/Auth.API"
RUN dotnet build "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# PUBLISH
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# RUN
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]