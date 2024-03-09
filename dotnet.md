# .NET

## Development Dockerfile

### `Dockerfile.dev`

```dockerfile
# Use dotnet sdk for restore steps
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore

WORKDIR /src

# Copy all source files
COPY ./MyWebApp/ ./MyWebApp

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
