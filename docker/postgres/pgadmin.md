## Running pgAdmin in a docker container

### Full documentation here: https://www.pgadmin.org/docs/pgadmin4/latest/container_deployment.html

### Running a simple container locally
```powershell
docker run `
--name pgadmin `
--restart unless-stopped `
-e PGADMIN_DEFAULT_EMAIL=admin@admin.com `
-e PGADMIN_DEFAULT_PASSWORD=String1@ `
-p 5050:80 `
-v C:/ProgramData/docker-volumes/pgadmin:/var/lib/pgadmin `
-d dpage/pgadmin4
```

## Connect to a PostgreSQL container instance

1. __Inside of a web browser launch:__ http://localhost:5050/browser/

2. __Sign in using the `PGADMIN_DEFAULT_EMAIL` and `PGADMIN_DEFAULT_PASSWORD`, e.g. `admin@admin.com` and `String1@`__

3. __Right click on__ `Servers` > `Register` > `Server...`

4. __Under__ `Connection`

    `Host name/address` - `host.docker.internal`

    `Port` - port on the host of the PostgreSQL container, e.g. `5433`

    `Username` - the `POSTGRES_USER` value, e.g. `admin`

    `Password` - the `POSTGRES_PASSWORD` value, e.g. `String1@`
