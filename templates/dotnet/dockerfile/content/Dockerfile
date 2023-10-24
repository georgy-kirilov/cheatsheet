# Use ASP.NET runtime for final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Use SDK image for build steps
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Restore
COPY ./__ProjectName__/__ProjectName__.csproj ./__ProjectName__/
RUN dotnet restore ./__ProjectName__/__ProjectName__.csproj

# Copy all other files and build
COPY . .
WORKDIR /src/__ProjectName__
RUN dotnet build "__ProjectName__.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "__ProjectName__.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "__ProjectName__.dll"]
