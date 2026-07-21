# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN apt-get update \
    && apt-get install -y --no-install-recommends nodejs npm \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /src
COPY Czytnik.sln ./
COPY Czytnik/Czytnik.csproj Czytnik/
COPY Czytnik_DataAccess/Czytnik_DataAccess.csproj Czytnik_DataAccess/
COPY Czytnik_Model/Czytnik_Model.csproj Czytnik_Model/
RUN dotnet restore Czytnik/Czytnik.csproj
COPY . .
RUN dotnet publish Czytnik/Czytnik.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "Czytnik.dll"]
