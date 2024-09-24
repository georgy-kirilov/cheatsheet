### Create new dotnet API project

```powershell
mkdir MyWebApp
cd ./MyWebApp
dotnet new sln -n MyWebApp
dotnet new web -n Api
dotnet sln add Api
dotnet new gitignore
dotnet new nuget.config
echo "" >> .env.example
```

### Remove `launchSettings.json` file
```csproj
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <!-- Add this setting to the .csproj file -->
    <NoDefaultLaunchSettingsFile>true</NoDefaultLaunchSettingsFile>
  </PropertyGroup>
</Project>
```
