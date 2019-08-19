FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview8-bionic AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.DataContract/*.csproj", "src/BuildingBlocks/CRM.DataContract/"]
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/Communication/CRM.Communication.Api/*.csproj", "src/Communication/CRM.Communication.Api/"]
COPY ["src/Communication/CRM.Communication.FunctionalTests/*.csproj", "src/Communication/CRM.Communication.FunctionalTests/"]
RUN dotnet restore src/Communication/CRM.Communication.Api/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503
RUN dotnet restore src/Communication/CRM.Communication.FunctionalTests/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503

COPY ["src/proto/.", "src/proto/"]

COPY ["src/BuildingBlocks/CRM.DataContract/.", "src/BuildingBlocks/CRM.DataContract/"]
COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/Communication/CRM.Communication.Api/.", "src/Communication/CRM.Communication.Api/"]
COPY ["src/Communication/CRM.Communication.FunctionalTests/.", "src/Communication/CRM.Communication.FunctionalTests/"]
RUN dotnet publish src/Communication/CRM.Communication.Api/ -c Release -o /app --no-restore

FROM builder AS functionaltest
WORKDIR /src/src/Communication/CRM.Communication.FunctionalTests

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview8-bionic
WORKDIR /app

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .
EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Communication.Api.dll"]
