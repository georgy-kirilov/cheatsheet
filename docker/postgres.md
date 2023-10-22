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

### Running PostgreSQL as a docker compose service

__`docker-compose.yml`__
```yml
services:
  database:
    container_name: application-database
    image: postgres:16
    restart: unless-stopped
    ports:
      - "${DB_PORT}:5432"
    environment:
      - POSTGRES_USER=${DB_USER}
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=${DB_NAME}
    volumes:
      - ${DB_VOLUME_PATH}:/var/lib/postgresql/data
```

__`docker-compose.override.yml`__
```yml
services:
  database:
    restart: on-failure
```

__`.env`__
```.env
DB_PORT=5433
DB_USER=admin
DB_PASSWORD=String1@
DB_NAME=application
DB_VOLUME_PATH=C:/ProgramData/docker-volumes/application/database
```

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
