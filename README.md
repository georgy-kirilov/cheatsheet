### Install template

```
dotnet new install ./templates/dotnet/modular-monolith --force
```

### Use template

```
dotnet new modmon -n MyCoolApplication
```

Working with database migrations

Enter the running API container:

```powershell
cd /your/docker-compose/file/path
docker-compose up --build
docker-compose exec api -it /bin/bash
dotnet ef migrations add InitialCreate --project ../Accounts -o Database/Migrations
dotnet ef database update
```
