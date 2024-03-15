# SQL Server

## Docker compose service

### `docker-compose.yml:`

```yml
services:
  sqlserver:
    container_name: sqlserver
    hostname: sqlserver
    restart: unless-stopped
    ports:
      - ${SQL_PORT}:1433
    environment:
      - ACCEPT_EULA=Y
      - MSSQL_DATA_DIR=/var/opt/sqlserver/data
      - MSSQL_LOG_DIR=/var/opt/sqlserver/log
      - MSSQL_BACKUP_DIR=/var/opt/sqlserver/backup
      - SA_PASSWORD=${MSSQL_SA_PASSWORD}
      - MSSQL_DB_PASS=${MSSQL_SA_PASSWORD}
      - MSSQL_DB_USER=${MSSQL_DB_USER}
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
MSSQL_HOST=sqlserver
MSSQL_PORT=1433
MSSQL_DB_USER=sa
MSSQL_SA_PASSWORD=Qwerty1@
```
