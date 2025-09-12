# MatchmakerPro – Deployment Checklist (Ubuntu + systemd + Nginx + HTTPS)

This checklist lets you deploy **matchmakerpro** (ASP.NET Core) on Ubuntu as a **systemd** service behind **Nginx** with **Let’s Encrypt** TLS.

> Project/app name: `matchmakerpro`  
> DLL: `matchmakerpro.dll`  
> Domain: `matchmakerpro.dk`

---

## 0) One-time server prep

- [ ] Update & upgrade
  ```bash
  sudo apt update && sudo apt upgrade -y
  ```
- [ ] Install runtime, Nginx, firewall
  ```bash
  sudo apt install -y dotnet-runtime-8.0 nginx ufw
  ```
- [ ] Open firewall
  ```bash
  sudo ufw allow OpenSSH
  sudo ufw allow 'Nginx Full'
  sudo ufw --force enable
  sudo ufw status verbose
  ```

---

## 1) Build & publish (run in repo on dev/CI)

- [ ] Publish Release
  ```bash
  dotnet publish -c Release -o publish
  ```
  *Optional self‑contained build:*
  ```bash
  dotnet publish -c Release -o publish -r linux-x64 --self-contained true /p:PublishSingleFile=true
  ```

---

## 2) App user & directory (first time on server)

- [ ] Create user + dir
  ```bash
  export APP_NAME=matchmakerpro
  export APP_DIR=/var/www/$APP_NAME

  sudo useradd -r -s /bin/false $APP_NAME
  sudo mkdir -p $APP_DIR
  sudo chown -R $APP_NAME:$APP_NAME $APP_DIR
  ```

---

## 3) Deploy artifacts to server

- [ ] Rsync publish folder
  ```bash
  # On your machine:
  rsync -avz --delete publish/ user@SERVER:/var/www/matchmakerpro/
  ```
- [ ] Fix ownership (if needed)
  ```bash
  sudo chown -R matchmakerpro:matchmakerpro /var/www/matchmakerpro/
  ```

---

## 4) systemd service

- [ ] Create unit
  ```bash
  sudo nano /etc/systemd/system/matchmakerpro.service
  ```

  Paste:
  ```ini
  [Unit]
  Description=ASP.NET Core app - matchmakerpro
  After=network.target

  [Service]
  User=matchmakerpro
  Group=matchmakerpro
  WorkingDirectory=/var/www/matchmakerpro
  ExecStart=/usr/bin/dotnet /var/www/matchmakerpro/matchmakerpro.dll

  Environment=ASPNETCORE_URLS=http://127.0.0.1:5000
  Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
  # Environment=ASPNETCORE_ENVIRONMENT=Production
  # EnvironmentFile=/etc/matchmakerpro.env

  Restart=always
  RestartSec=5
  SyslogIdentifier=matchmakerpro

  NoNewPrivileges=true
  ProtectSystem=full
  ProtectHome=true
  PrivateTmp=true

  [Install]
  WantedBy=multi-user.target
  ```

  *If self‑contained build:* set `ExecStart=/var/www/matchmakerpro/matchmakerpro`

- [ ] Enable & start
  ```bash
  sudo systemctl daemon-reload
  sudo systemctl enable matchmakerpro
  sudo systemctl start matchmakerpro
  sudo systemctl status matchmakerpro --no-pager
  ```

---

## 5) Nginx reverse proxy (HTTP)

- [ ] Site config
  ```bash
  sudo nano /etc/nginx/sites-available/matchmakerpro.conf
  ```

  Paste:
  ```nginx
  server {
      listen 80;
      listen [::]:80;
      server_name matchmakerpro.dk www.matchmakerpro.dk;

      client_max_body_size 20m;

      location / {
          proxy_pass         http://127.0.0.1:5000;
          proxy_http_version 1.1;

          proxy_set_header   Upgrade $http_upgrade;
          proxy_set_header   Connection keep-alive;
          proxy_set_header   Host $host;
          proxy_set_header   X-Real-IP $remote_addr;
          proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
          proxy_set_header   X-Forwarded-Proto $scheme;
          proxy_cache_bypass $http_upgrade;
      }
  }
  ```

- [ ] Enable & reload
  ```bash
  sudo ln -s /etc/nginx/sites-available/matchmakerpro.conf /etc/nginx/sites-enabled/matchmakerpro.conf
  sudo nginx -t
  sudo systemctl reload nginx
  ```

- [ ] Quick checks
  ```bash
  curl -I http://127.0.0.1:5000
  curl -I http://matchmakerpro.dk
  ```

---

## 6) HTTPS (Let’s Encrypt)

- [ ] Install certbot
  ```bash
  sudo snap install core; sudo snap refresh core
  sudo snap install --classic certbot
  sudo ln -s /snap/bin/certbot /usr/bin/certbot
  ```

- [ ] Obtain cert
  ```bash
  sudo certbot --nginx -d matchmakerpro.dk -d www.matchmakerpro.dk
  ```

- [ ] Verify auto‑renew
  ```bash
  systemctl list-timers | grep certbot || sudo systemctl status snap.certbot.renew.service
  ```

---

## 7) Environment variables & secrets (optional)

- [ ] Create env file
  ```bash
  sudo bash -c 'cat >/etc/matchmakerpro.env <<EOF
  ASPNETCORE_ENVIRONMENT=Production
  # ConnectionStrings__Default=Server=localhost;Database=app;User Id=app;Password=StrongPwd!;
  EOF'
  sudo chmod 600 /etc/matchmakerpro.env
  ```

- [ ] Enable in unit
  - Uncomment `EnvironmentFile=/etc/matchmakerpro.env` in `/etc/systemd/system/matchmakerpro.service`
  - Reload & restart:
    ```bash
    sudo systemctl daemon-reload
    sudo systemctl restart matchmakerpro
    ```

---

## 8) Updating to a new release

- [ ] Publish new build
  ```bash
  dotnet publish -c Release -o publish
  ```
- [ ] Deploy
  ```bash
  rsync -avz --delete publish/ user@SERVER:/var/www/matchmakerpro/
  ```
- [ ] Restart app
  ```bash
  sudo systemctl restart matchmakerpro
  ```
- [ ] Smoke test
  ```bash
  curl -I https://matchmakerpro.dk
  ```

---

## 9) Troubleshooting quick refs

- App logs
  ```bash
  sudo journalctl -u matchmakerpro -f --no-pager
  ```
- Nginx syntax / reload
  ```bash
  sudo nginx -t && sudo systemctl reload nginx
  ```
- Nginx error log
  ```bash
  sudo tail -n 200 /var/log/nginx/error.log
  ```
- Firewall
  ```bash
  sudo ufw status verbose
  ```
- Service not starting?
  - Check `ExecStart` path (DLL vs self‑contained)
  - Check permissions/ownership in `/var/www/matchmakerpro/`
  - Verify `dotnet-runtime-8.0` installed (if not self‑contained)

---

## 10) Rollback (simple)

- Keep last two releases:
  ```bash
  /var/www/matchmakerpro/releases/2025-09-11/
  /var/www/matchmakerpro/releases/prev/
  ```
- Symlink `current` → desired release, then restart:
  ```bash
  ln -sfn /var/www/matchmakerpro/releases/prev /var/www/matchmakerpro/current
  sudo systemctl restart matchmakerpro
  ```

---

## 11) Security hardening notes (optional)

- Run as non‑login user (`matchmakerpro`) ✔
- Keep `NoNewPrivileges`, `ProtectSystem`, `ProtectHome`, `PrivateTmp` ✔
- Limit `client_max_body_size` in Nginx (adjust as needed)
- Use strong DB creds via `/etc/matchmakerpro.env` (600 perms)
- Regular OS & package updates; renew certs

---

## 12) Health checks (optional)

- Add a lightweight endpoint (e.g. `/healthz`) in the app.
- Nginx local probe:
  ```bash
  curl -sSf http://127.0.0.1:5000/healthz || echo "health check failed"
  ```

---

**Done!** Keep this file in your repo as `README-deploy.md`. Update paths/domains if your infra changes.
