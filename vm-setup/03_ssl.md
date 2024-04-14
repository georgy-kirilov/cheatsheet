mkdir /srv/honest-auto
mkdir /srv/honest-auto/data/certs
mkdir /srv/honest-auto/data/www
mkdir /srv/honest-auto/nginx

copy the compose.yml into /srv/honest-auto
copy the nginx.certbot.conf into /srv/honest-auto/nginx as nginx.conf

start compose.yml:
```
docker compose up -d
```

inside `/srv/honest-auto/` run:

```bash
docker run --rm \
    -v "$(pwd)/data/certs:/etc/letsencrypt" \
    -v "$(pwd)/data/www:/var/www/certbot" \
    certbot/certbot certonly \
    --webroot \
    --webroot-path=/var/www/certbot \
    -d beloteplay.site \
    --email georgi.n.kirilov@gmail.com \
    --agree-tos \
    --no-eff-email
```

```
docker compose down
```

copy the nginx.conf into /srv/honest-auto/nginx as nginx.conf

```bash
docker compose up
```
