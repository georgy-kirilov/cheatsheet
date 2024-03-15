# SQL Server

## Docker compose service

### `compose.yml:`

```yml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: sqlserver
    hostname: sqlserver
    restart: unless-stopped
    ports:
      - 1444:1433
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=${MSSQL_SA_PASSWORD}
      - MSSQL_DB_PASS=${MSSQL_DB_PASSWORD}
      - MSSQL_DB_USER=${MSSQL_DB_USER}
      - MSSQL_DATA_DIR=/var/opt/sqlserver/data
      - MSSQL_LOG_DIR=/var/opt/sqlserver/log
      - MSSQL_BACKUP_DIR=/var/opt/sqlserver/backup
    volumes:
      - sqlsystem:/var/opt/mssql
      - sqldata:/var/opt/sqlserver/data
      - sqllog:/var/opt/sqlserver/log
      - sqlbackup:/var/opt/sqlserver/backup

volumes:
  sqlsystem:
  sqldata:
  sqllog:
  sqlbackup:
```

### `.env:`
```env
MSSQL_DB_USER=sa
MSSQL_DB_PASSWORD=P@55W0rd
MSSQL_SA_PASSWORD=Qwerty1@
```
