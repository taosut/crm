FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview8-bionic AS builder

ARG feed='--source "https://api.nuget.org/v3/index.json"'

WORKDIR /src
COPY ["src/BuildingBlocks/CRM.Shared/*.csproj", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Migration/*.csproj", "src/BuildingBlocks/CRM.Migration/"]
COPY ["src/Contact/CRM.Contact.Contract/*.csproj", "src/Contact/CRM.Contact.Contract/"]
COPY ["src/Contact/CRM.Contact.Api/*.csproj", "src/Contact/CRM.Contact.Api/"]

RUN dotnet restore src/Contact/CRM.Contact.Api/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503
RUN dotnet restore src/BuildingBlocks/CRM.Migration/ /property:Configuration=Release ${feed} -nowarn:msb3202,nu1503

COPY ["src/BuildingBlocks/CRM.Shared/.", "src/BuildingBlocks/CRM.Shared/"]
COPY ["src/BuildingBlocks/CRM.Migration/.", "src/BuildingBlocks/CRM.Migration/"]
COPY ["src/Contact/CRM.Contact.Contract/.", "src/Contact/CRM.Contact.Contract/"]
COPY ["src/Contact/CRM.Contact.Api/.", "src/Contact/CRM.Contact.Api/"]
RUN dotnet publish src/Contact/CRM.Contact.Api/ -c Release -o /app --no-restore

FROM builder AS migration
WORKDIR /src/src/BuildingBlocks/CRM.Migration/

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview8-bionic
WORKDIR /app

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .

EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Contact.Api.dll"]