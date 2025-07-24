FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
RUN apt-get update && apt-get install -y libsqlite3-dev
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopier alt inn i containeren (hele l�sningen)
COPY . .

WORKDIR /src/MasterMindAPI
RUN dotnet restore "./MasterMindAPI.csproj"
RUN dotnet build "./MasterMindAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MasterMindAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Kopier sertifikat (ligger p� roten)
COPY devCertificate.pfx .

# Sett milj�variabler for HTTPS
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=crypticpassword
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/devCertificate.pfx

ENTRYPOINT ["dotnet", "MasterMindAPI.dll"]