FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-bionic AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.Migration/*.csproj", "src/BuildingBlocks/CRM.Migration/"]
RUN dotnet restore src/BuildingBlocks/CRM.Migration/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503

COPY ["src/BuildingBlocks/CRM.Migration/.", "src/BuildingBlocks/CRM.Migration/"]
RUN dotnet publish src/BuildingBlocks/CRM.Migration/ -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-bionic
WORKDIR /app

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .
