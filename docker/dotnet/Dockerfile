# Use ASP.NET runtime for final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app

# Expose port 80
EXPOSE 80

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Use SDK image for build steps
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore

# Change working directory
WORKDIR /src/Api

# Build the project
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
