FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PROG7311 POE.csproj", "./"]

RUN dotnet restore "PROG7311 POE.csproj"

COPY . .

RUN dotnet build "PROG7311 POE.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "PROG7311 POE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PROG7311 POE.dll"]