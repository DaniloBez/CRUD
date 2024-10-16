# CRUD API Project

## Опис
Це проект для створення API, яке забезпечує управління користувачами за допомогою CRUD-операцій (створення, читання, оновлення, видалення). Також реалізована аутентифікація користувачів за допомогою JWT токенів. Дані можуть зберігатися у базі даних SQLite або у JSON файлі в залежності від налаштувань.

## Вимоги
Перед початком роботи переконайтеся, що у вас встановлено:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/downloads)

## Інструкція щодо запуску проекту
### Крок 1: Клонування репозиторію
Клонуйте цей репозиторій на свою локальну машину:
```bash
git clone https://github.com/DaniloBez/CRUD.git
```

### Крок 2: Встановлення залежностей
Перейдіть до папки проекту:
```bash
cd CRUD
```
Встановіть всі необхідні залежності за допомогою .NET CLI:
```bash
dotnet restore
```

### Крок 3: Запуск проекту
Запустіть API сервер за допомогою команди:
```bash
dotnet run --project CRUD.API/CRUD.API.csproj
```

### Крок 4: Перевірка Swagger документації
Відкрийте браузер і перейдіть за адресою:
<http://localhost:5108/swagger/index.html>

## Перемикання між SQL і JSON зберіганням
Проект дозволяє використовувати два типи репозиторіїв для збереження даних: SQL або JSON. Тип репозиторію можна налаштувати через файл `appsettings.json`, у поле RepositoryType введіть потрібний тип зберігання данних: або `SQL` або `Json`. 
### Налаштування через `appsettings.json`
```json
{
  "Jwt": {
    "Key": "uM+/d/3E5gkhZgY5nsP4mRXWwz3BHRy/sD2rfwK4q/g=",
    "Issuer": "MyIssuer",
    "Audience": "MyAudience"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "RepositoryType": "SQL" // Можливі варіанти: 'SQL' або 'Json'
}
```

## Опис функціоналу API
### Маршрути
#### Аутентифікація:
- **POST /api/Auth/login** — Логін користувача з використанням JWT.
  - **Параметри:** JSON з полями `Username` та `Password`.
  - **Відповідь:** JWT токен.

#### Користувачі:
- **GET /User** — Отримати список всіх користувачів.
- **GET /User/{nickName}** — Отримати користувача за NickName.
  - **Вимагає авторизацію:** (Bearer токен).
- **POST /User** — Створити нового користувача.
  - **Параметри:** JSON з даними користувача.
- **PUT /User** — Оновити інформацію користувача.
  - **Вимагає авторизацію:** (Bearer токен).
- **DELETE /User** — Видалити свій обліковий запис.
  - **Вимагає авторизацію:** (Bearer токен).

## Приклад авторизації
1. Щоб авторизуватися у вас повинен бути створений юзер (Post/user)
2. Використовуйте POST-запит до `/api/Auth/login` з вірними обліковими даними.
3. Додайте токен до заголовку `Authorization` у форматі: `Bearer your_token`.

Тепер ви маєте доступ до захищеного функціоналу.

## Важливо
- При реєстрації нового користувача, пароль автоматично хешується перед збереженням.
- Для використання захищених маршрутів, необхідно спершу отримати JWT токен через `/api/Auth/login`.
