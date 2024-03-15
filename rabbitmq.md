# RabbitMQ

## Simple Docker Container

```bash
docker run \
  --name local_rabbitmq \
  --hostname local_rabbitmq \
  --restart unless-stopped \
  -p 5672:5672 \
  -p 15672:15672 \
  -e RABBITMQ_DEFAULT_USER=guest \
  -e RABBITMQ_DEFAULT_PASS=guest \
  -v local_rabbitmq_data:/var/lib/rabbitmq \
  -d rabbitmq:3.13.0-management
```

## Docker Compose Service

### `.env`
```env
RABBITMQ_USER=guest
RABBITMQ_PASSWORD=guest
```

### `compose.yml`
```yml
services:
  rabbitmq:
    image: rabbitmq:3.13.0-management
    container_name: rabbitmq
    hostname: rabbitmq
    restart: unless-stopped
    ports:
      - "5673:5672"
      - "15673:15672"
    environment:
      - RABBITMQ_DEFAULT_USER=${RABBITMQ_USER}
      - RABBITMQ_DEFAULT_PASS=${RABBITMQ_PASSWORD}
      - RABBITMQ_SERVER_ADDITIONAL_ERL_ARGS=-rabbitmq_management listener [{port,15672}]
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq
    healthcheck:
      test: ["CMD-SHELL", "rabbitmq-diagnostics -q check_running"]
      interval: 10s
      timeout: 5s
      retries: 5
      start_period: 10s

volumes:
  rabbitmq_data:
```
