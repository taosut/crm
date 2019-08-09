FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview7-bionic AS builder
WORKDIR /src
COPY ["src/CRM.DataContract/*.csproj", "src/CRM.DataContract/"]
COPY ["src/CRM.Shared/*.csproj", "src/CRM.Shared/"]
COPY ["src/CRM.Graph.Gateway/*.csproj", "src/CRM.Graph.Gateway/"]
RUN dotnet restore src/CRM.Graph.Gateway/ /property:Configuration=Release

COPY ["src/proto/.", "src/proto/"]

COPY ["src/CRM.DataContract/.", "src/CRM.DataContract/"]
COPY ["src/CRM.Shared/.", "src/CRM.Shared/"]
COPY ["src/CRM.Graph.Gateway/.", "src/CRM.Graph.Gateway/"]
RUN dotnet publish src/CRM.Graph.Gateway/ -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview7-bionic
WORKDIR /app

ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

COPY --from=builder /app .

EXPOSE 80
ENTRYPOINT [ "dotnet",  "CRM.Graph.Gateway.dll"]