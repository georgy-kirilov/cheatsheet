# Running pgAdmin4 in a docker container

### View the full documentation here:
https://www.pgadmin.org/docs/pgadmin4/latest/container_deployment.html

### Command template
```
docker run \
--name <container-name> \
--restart always \
-p <port-on-host>:80 \
-e PGADMIN_DEFAULT_EMAIL=<admin-email> \
-e PGADMIN_DEFAULT_PASSWORD=<admin-password> \
-v <absolute-path-to-volume-on-host>:/var/lib/pgadmin `
-d dpage/pgadmin4
```

### Example in Powershell
```
docker run `
--name pgadmin `
--restart always `
-p 5050:80 `
-e PGADMIN_DEFAULT_EMAIL=admin@admin.com `
-e PGADMIN_DEFAULT_PASSWORD=String1@ `
-v C:/ProgramData/docker_volumes/pgadmin:/var/lib/pgadmin `
-d dpage/pgadmin4
```

## Connect to a PostgreSQL container instance:

1. __Launch__ `http://localhost:5333` __inside of a browser__

2. __Right click on__ `Servers` > `Register` > `Server...`

3. __Under__ `General`
   
    `Name` - the name of the PostgreSQL container (e.g. `postgres`)

5. __Under__ `Connection`

    `Host name/address` - `host.docker.internal`

    `Port` - the host port of the PostgreSQL container (e.g. `5444`)

    `Username` - the PostgreSQL container POSTGRES_USER value (e.g. `postgres`)

    `Password` - the PostgreSQL container POSTGRES_PASSWORD (e.g. `String1@`)
