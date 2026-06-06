# =========================
# BASE IMAGE (runtime)
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

# =========================
# BUILD IMAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy ONLY csproj first (for caching restore)
COPY PROG7311.API.csproj ./
RUN dotnet restore PROG7311.API.csproj

# Copy everything else
COPY . .

# Build project
RUN dotnet build PROG7311.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# =========================
# PUBLISH IMAGE
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish PROG7311.API.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# FINAL RUNTIME IMAGE
# =========================
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PROG7311.API.dll"]