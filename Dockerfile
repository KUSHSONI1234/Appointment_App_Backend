# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY RegisterAPI.csproj ./
RUN dotnet restore

# Copy all files and publish
COPY . ./
RUN dotnet publish RegisterAPI.csproj -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "RegisterAPI.dll"]
