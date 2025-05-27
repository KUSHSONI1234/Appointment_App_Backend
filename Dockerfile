# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY RegisterAPI/RegisterAPI.csproj ./RegisterAPI/
WORKDIR /app/RegisterAPI
RUN dotnet restore

# Copy all source files and publish
COPY RegisterAPI/. ./
RUN dotnet publish RegisterAPI.csproj -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/RegisterAPI/out ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "RegisterAPI.dll"]
