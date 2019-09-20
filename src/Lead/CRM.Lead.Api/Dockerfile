FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-rc1-bionic AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.DataContract/*.csproj", "src/BuildingBlocks/CRM.DataContract/"]
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/Lead/CRM.Lead.Api/*.csproj", "src/Lead/CRM.Lead.Api/"]
COPY ["src/Lead/CRM.Lead.FunctionalTests/*.csproj", "src/Lead/CRM.Lead.FunctionalTests/"]

RUN dotnet restore src/Lead/CRM.Lead.Api/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503
RUN dotnet restore src/Lead/CRM.Lead.FunctionalTests/ /property:Configuration=Debug ${feed} -nowarn:msb3202,nu1503

COPY ["src/proto/.", "src/proto/"]
COPY ["src/BuildingBlocks/CRM.DataContract/.", "src/BuildingBlocks/CRM.DataContract/"]
COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/Lead/CRM.Lead.Api/.", "src/Lead/CRM.Lead.Api/"]
COPY ["src/Lead/CRM.Lead.FunctionalTests/.", "src/Lead/CRM.Lead.FunctionalTests/"]
RUN dotnet publish src/Lead/CRM.Lead.Api/ -c Release -o /app --no-restore

FROM builder AS functionaltest
WORKDIR /src/src/Lead/CRM.Lead.FunctionalTests

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-rc1-bionic
WORKDIR /app

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .

EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Lead.Api.dll"]
