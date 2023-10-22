## Running PostgreSQL in docker

### Full documentation here: https://hub.docker.com/_/postgres/

### Running a simple container locally

```powershell
docker run `
--name postgres-database `
--restart unless-stopped `
-e POSTGRES_USER=admin `
-e POSTGRES_PASSWORD=String1@ `
-e POSTGRES_DB=postgres `
-p 5433:5432 `
-v C:/ProgramData/docker-volumes/postgres:/var/lib/postgresql/data `
-d postgres:16
```
