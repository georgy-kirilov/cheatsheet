# .NET

## Nuget Config
### `nuget.config`
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
  </packageSources>
</configuration>
```

## Development Dockerfile
### `Dockerfile.dev`
```dockerfile
# Use dotnet sdk for restore steps
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore

WORKDIR /src

# Copy all source files
COPY ./MyWebApp/ ./MyWebApp
COPY ./nuget.config ./nuget.config

# Restore dependencies
RUN dotnet restore ./MyWebApp/MyWebApp.csproj

# Use the same image for development
FROM restore AS development

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_HTTP_PORTS=80
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

# Set working directory
WORKDIR /src/MyWebApp

# Install dotnet EF tool and add it to PATH
RUN dotnet tool install --global dotnet-ef --version 8.0.2
ENV PATH="${PATH}:/root/.dotnet/tools"

# Watch for changes and restart app
ENTRYPOINT ["dotnet", "watch", "run", "--no-restore"]
```

## Production Dockerfile
### `Dockerfile.prod`
```dockerfile
# Use dotnet runtime for final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Run as a non-root user for security
RUN useradd -m appuser
USER appuser

# Use dotnet sdk for build steps
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["nuget.config", "."]

# Copy csproj files for all projects
COPY ["./MyWebApp/MyWebApp.csproj", "./MyWebApp/"]

# Restore
RUN dotnet restore "./MyWebApp/MyWebApp.csproj"

# Copy the rest of the source code
COPY . .

# Build
WORKDIR "/src/MyWebApp"
RUN dotnet build "MyWebApp.csproj" -c Release -o /app/build --no-restore

# Publish
FROM build AS publish
RUN dotnet publish "MyWebApp.csproj" -c Release -o /app/publish --no-build

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebApp.dll"]
```
