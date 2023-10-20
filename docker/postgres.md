# Running a PostgreSQL docker container

## Command template
```
docker run \
-d \
--name <container-name> \
--restart always \
-e POSTGRES_PASSWORD=<admin-password> \
-e POSTGRES_USER=<admin-user-name> \
-e POSTGRES_DB=<default-database-name> \
-p <port-on-host>:5432 \
-v <absolute-path-to-volume-on-host>:/var/lib/postgresql/data \
postgres:<version>
```

## Example in Powershell
```
docker run `
-d `
--name postgres `
--restart always `
-e POSTGRES_PASSWORD=String1@ `
-e POSTGRES_USER=postgres `
-e POSTGRES_DB=postgres `
-p 5444:5432 `
-v C:/docker/volumes/postgres/data:/var/lib/postgresql/data `
postgres:16
```
