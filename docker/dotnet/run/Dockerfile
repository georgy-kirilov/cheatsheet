FROM mcr.microsoft.com/dotnet/sdk:9.0 AS restore
WORKDIR /src
EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Development

COPY ["Application/Application.csproj", "Application/"]
COPY ["nuget.config", "Application/"]
RUN dotnet restore "Application/Application.csproj"
COPY . .

RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

FROM restore AS development
WORKDIR "/src/Application"
RUN dotnet build "Application.csproj" --no-restore -c Debug

ENTRYPOINT ["dotnet", "run", "--no-restore", "-c", "Debug"]
