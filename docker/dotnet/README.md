### Create new dotnet API project

```powershell
mkdir MyWebApp; `
cd ./MyWebApp; `
echo "" >> .env.example; `
dotnet new web -n Api; `
cd ./Api; `
dotnet new sln -n MyWebApp; `
dotnet sln add Api.csproj; `
dotnet new gitignore; `
dotnet new nuget.config; `
rm -r ./Properties
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
