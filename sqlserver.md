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
      - "1444:1433"
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=${MSSQL_SA_PASSWORD}
      - MSSQL_DB_PASS=${MSSQL_DB_PASSWORD}
      - MSSQL_DB_USER=${MSSQL_DB_USER}
      - MSSQL_DATA_DIR=/var/opt/sqlserver/data
      - MSSQL_LOG_DIR=/var/opt/sqlserver/log
      - MSSQL_BACKUP_DIR=/var/opt/sqlserver/backup
    volumes:
      - sqlserver_system:/var/opt/mssql
      - sqlserver_data:/var/opt/sqlserver/data
      - sqlserver_log:/var/opt/sqlserver/log
      - sqlserver_backup:/var/opt/sqlserver/backup
    healthcheck:
      test: [ "CMD", "/opt/mssql-tools/bin/sqlcmd", "-U", "sa", "-P", "${MSSQL_SA_PASSWORD}", "-Q", "SELECT 1" ]
      interval: 10s
      timeout: 5s
      retries: 5
      start_period: 10s

volumes:
  sqlserver_system:
  sqlserver_data:
  sqlserver_log:
  sqlserver_backup:
```

### `.env:`
```env
MSSQL_DB_USER=sa
MSSQL_DB_PASSWORD=P@55W0rd
MSSQL_SA_PASSWORD=Qwerty1@
```
