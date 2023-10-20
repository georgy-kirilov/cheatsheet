# Running a PostgreSQL docker container

### View the full documentation here: https://hub.docker.com/_/postgres/

## Command template
```
docker run \
--name <container-name> \
--restart always \
-e POSTGRES_PASSWORD=<admin-password> \
-e POSTGRES_USER=<admin-user-name> \
-e POSTGRES_DB=<default-database-name> \
-p <port-on-host>:5432 \
-v <absolute-path-to-volume-on-host>:/var/lib/postgresql/data \
-d postgres:<version>
```

## Example in Powershell
```
docker run `
--name postgres `
--restart always `
-e POSTGRES_PASSWORD=String1@ `
-e POSTGRES_USER=postgres `
-e POSTGRES_DB=postgres `
-p 5444:5432 `
-v C:/docker/volumes/postgres/data:/var/lib/postgresql/data `
-d postgres:16
```
