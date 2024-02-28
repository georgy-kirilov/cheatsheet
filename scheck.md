# RabbitMQ

## Open Powershell as Administrator

### Run a new RabbitMQ docker container:
```powershell
docker run -d `
  --restart unless-stopped `
  --name smartcheck_rabbitmq `
  --hostname smartcheck_rabbitmq `
  -p 5672:5672 `
  -p 15672:15672 `
  -e RABBITMQ_DEFAULT_USER=guest `
  -e RABBITMQ_DEFAULT_PASS=guest `
  -v smartcheck_rabbitmq_data:/var/lib/rabbitmq `
  rabbitmq:3.13.0-management
```

### After the container has been started, enabe `Management` and `Tracing`:
```
docker exec smartcheck_rabbitmq rabbitmq-plugins enable rabbitmq_management
docker exec smartcheck_rabbitmq rabbitmq-plugins enable rabbitmq_tracing
```

### Add a new `SmartCheck` RabbitMQ user:
```powershell
docker exec smartcheck_rabbitmq rabbitmqctl add_user scheck scheck
docker exec smartcheck_rabbitmq rabbitmqctl set_permissions -p / scheck ".*" ".*" ".*"
docker exec smartcheck_rabbitmq rabbitmqctl set_user_tags scheck administrator
```
