# Running a pgAdmin docker container

### View the full documentation here: https://www.pgadmin.org/docs/pgadmin4/latest/container_deployment.html

## Command template
```
docker run \
--name <container-name> \
--restart always \
-p <port-on-host>:80 \
-e PGADMIN_DEFAULT_EMAIL=<admin-email> \
-e PGADMIN_DEFAULT_PASSWORD=<admin-password> \
-d dpage/pgadmin4
```

## Example in Powershell
```
docker run `
--name pgadmin `
--restart always `
-p 5555:80 `
-e PGADMIN_DEFAULT_EMAIL=admin@admin.com `
-e PGADMIN_DEFAULT_PASSWORD=String1@ `
-d dpage/pgadmin4
```
