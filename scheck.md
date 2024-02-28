# RabbitMQ

## Open PowerShell as Administrator

```powershell
docker run -d `
  --name scheck_rabbitmq `
  --hostname scheck_rabbitmq `
  -p 5672:5672 `
  -p 15672:15672 `
  -e RABBITMQ_DEFAULT_USER=scheck `
  -e RABBITMQ_DEFAULT_PASS=scheck `
  -v scheck_rabbitmq_data:/var/lib/rabbitmq `
  rabbitmq:3.13.0-management
```
