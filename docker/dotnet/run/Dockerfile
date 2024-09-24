# Use dotnet sdk for restore steps
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore

WORKDIR /src

# Copy only .csproj and nuget.config files to optimize docker layer caching
COPY ./Api/Api.csproj ./Api/
COPY ./Api/nuget.config ./Api/

# Restore dependencies
RUN dotnet restore ./Api/Api.csproj

# Copy all remaining source files
COPY ./Api/ ./Api/

# Install dotnet EF tool and add it to PATH
RUN dotnet tool install --global dotnet-ef --version 8.0.8
ENV PATH="${PATH}:/root/.dotnet/tools"

# Use the same image for development
FROM restore AS development

# Explicitly unset HTTP_PORTS to avoid overrides
ENV HTTP_PORTS=

# Set the environment to Development
ENV ASPNETCORE_ENVIRONMENT=Development

# Bind the app to port 80
ENV ASPNETCORE_URLS=http://+:80

# Set working directory
WORKDIR /src/Api

# Build the app in Debug mode
RUN dotnet build --configuration Debug

# Run the application
ENTRYPOINT ["dotnet", "run", "--no-restore", "--configuration", "Debug"]
