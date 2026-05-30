# Лабораторна робота №2

## Ваш варіант

| Параметр     | Значення                  |
| ------------ | ------------------------- |
| Застосунок   | Notes Service             |
| База даних   | PostgreSQL                |
| Порт         | 8000                      |
| Конфігурація | /etc/mywebapp/config.json |

---

# Що потрібно реалізувати

## 1. Dockerfile для mywebapp

Контейнер повинен:

* запускати Notes Service
* працювати на порту 8000
* використовувати PostgreSQL
* використовувати config.json

---

## 2. Docker Compose

Створити:

docker-compose.yml

---

# Через Docker Compose повинні запускатися

| Сервіс   | Призначення     |
| -------- | --------------- |
| nginx    | reverse proxy   |
| mywebapp | web application |
| postgres | база даних      |

---

## 3. PostgreSQL

PostgreSQL повинен:

* працювати в окремому контейнері
* зберігати дані після перезапуску контейнерів

Потрібно використати:

* Docker volume
  або
* bind mount

---

## 4. Nginx

Nginx повинен:

* слухати порт 80
* проксувати запити до mywebapp:8000

---

## 5. Docker Network

Усі сервіси повинні працювати:

* в окремій Docker network
* не в default network

---

## 6. Health Endpoints

Повинні працювати:

* GET /health/alive
* GET /health/ready

---

## 7. Notes API

Повинні працювати:

* GET /notes
* POST /notes
* GET /notes/<id>

---

# Що потрібно показати викладачу

## Запуск контейнерів

```bash id="c2x81n"
docker compose up --build
```

---

## Список контейнерів

```bash id="r5v4pa"
docker ps
```

---

## Роботу nginx

Відкрити:

```text id="n3q9wf"
http://localhost
```

---

## Роботу API

### Список нотаток

```bash id="d8j1lk"
curl http://localhost/notes
```

---

### Створення нотатки

```bash id="m4k2as"
curl -X POST http://localhost/notes
```

---

### Отримання нотатки

```bash id="p7y3er"
curl http://localhost/notes/1
```

---

## Health Endpoints

```bash id="t1f8uo"
curl http://localhost/health/alive
```

```bash id="h5z2cv"
curl http://localhost/health/ready
```

---

## Persistence PostgreSQL

Показати:

1. Створення нотатки
2. Перезапуск контейнерів
3. Дані залишилися

---

## Docker Network

```bash id="k9w3sm"
docker network ls
```

---

## Docker Volume

```bash id="e6a7qn"
docker volume ls
```

---

# Що має бути в GitHub

* Dockerfile
* docker-compose.yml
* nginx config
* README.md
