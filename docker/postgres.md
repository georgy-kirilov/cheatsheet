# Running a PostgreSQL docker container

### Full documentation here: https://hub.docker.com/_/postgres/

```
docker run `
--name postgres-local `
--restart always `
-e POSTGRES_USER=admin `
-e POSTGRES_PASSWORD=String1@ `
-e POSTGRES_DB=postgres `
-p 5433:5432 `
-v C:/ProgramData/docker-volumes/postgres-local:/var/lib/postgresql/data `
-d postgres:16
```
