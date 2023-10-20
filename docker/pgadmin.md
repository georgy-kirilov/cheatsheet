# pgAdmin in Docker

## Running pgAdmin4 in a docker container

### View the full documentation here
https://www.pgadmin.org/docs/pgadmin4/latest/container_deployment.html

### Command template
```
docker run \
--name <container-name> \
--restart always \
-p <port-on-host>:80 \
-e PGADMIN_DEFAULT_EMAIL=<admin-email> \
-e PGADMIN_DEFAULT_PASSWORD=<admin-password> \
-d dpage/pgadmin4
```

### Example in Powershell
```
docker run `
--name pgadmin `
--restart always `
-p 5333:80 `
-e PGADMIN_DEFAULT_EMAIL=admin@admin.com `
-e PGADMIN_DEFAULT_PASSWORD=String1@ `
-d dpage/pgadmin4
```

## Connect to a PostgreSQL docker container instance:
__Right click on__ `Servers` > `Register` > `Server...`

### Under `General`
`Name` - the name of the PostgreSQL container (e.g. `postgres`)

### Under `Connection`
`Host name/address` - `host.docker.internal`
`Port` - the host port of the PostgreSQL container (e.g. `5444`)
`Username` - the PostgreSQL container POSTGRES_USER value (e.g. `postgres`)
`Password` - the PostgreSQL container POSTGRES_PASSWORD (e.g. `String1@`)
