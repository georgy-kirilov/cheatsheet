# Use dotnet runtime for final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

# Explicitly unset HTTP_PORTS to avoid overrides
ENV HTTP_PORTS=

# Set the environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Bind the app to port 80
ENV ASPNETCORE_URLS=http://+:80

# Use dotnet sdk for build steps
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only .csproj and nuget.config files to optimize docker layer caching
COPY ./Api.csproj ./Api/
COPY ./nuget.config ./Api/

# Restore dependencies
RUN dotnet restore ./Api/Api.csproj

# Copy all remaining source files
COPY ./ ./Api/

# Build
WORKDIR /src/Api
RUN dotnet build "Api.csproj" -c Release -o /app/build --no-restore

# Publish
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]