
# 📘 Система управління технологічними подіями - Event Notifier

Вебзастосунок для управління технологічними подіями, що дозволяє користувачам переглядати IT-події, підписуватися на них і отримувати автоматичні нагадування через електронну пошту за кілька днів до події та безпосередньо в день її проведення.

---

## 👤 Автор

- **ПІБ**: Герус Наталія
- **Група**: ФЕІМ-24
- **Керівник**: Монастирський Любомир, доктор фізико-математичних 
наук
- **Дата виконання**: 23.12.2025

---

## 📌 Загальна інформація

- **Тип проєкту**: Вебзастосунок
- **Серверна частина**: .NET 8 (ASP.NET Web API)  
- **Клієнтська частина**: Angular  
- **Аутентифікація / Авторизація**: Duende IdentityServer + ASP.NET Core Identity  
- **Хмарна інфраструктура**: Microsoft Azure  
- **Обробка сповіщень**: Azure Functions (Timer Trigger + Queue Trigger)  
- **Черги повідомлень**: Azure Queue Storage  
- **Email-розсилки**: SMTP (SmtpClient)  
- **База даних**: Azure SQL Serverless  
- **Зберігання медіафайлів**: Azure Blob Storage  
- **Моніторинг та логування**: .NET Aspire



---

## 🧠 Опис функціоналу
- 🔐 Реєстрація, авторизація та керування обліковими записами користувачів  
- 📅 Перегляд каталогу подій за категоріями та фільтрами  
- ⭐ Підписка на події та перегляд збережених у персональному кабінеті  
- 📩 Автоматична email-розсилка нагадувань за 7 днів, за 1 день та у день події  
- ✏️ Адміністрування подій: створення, редагування, зміна дат, видалення  
- 🏷 Завантаження зображень подій та збереження у Azure Blob Storage  
- ⚙️ Подієва обробка через Azure Functions (Schedule/Reschedule/SendEmail)  
- 🟢 Реакція на зміну події та автоматичне перепланування нагадувань  
- 🔗 REST API для взаємодії фронтенду з backend-сервісами  
- 📊 Відстеження активності та робочих процесів через .NET Aspire


---

## 🧱 Опис основних проєктів 

| Проєкт / Файл                    | Призначення |
|----------------------------------|-------------|
| `EventNotifier`                  | Основний backend-API (ASP.NET Web API на .NET 8) для роботи з подіями: CRUD операції, керування підписками, формування повідомлень про зміну подій (EventUpdated) та взаємодія з чергою. |
| `EventNotifier.IdentityServer`   | Сервіс автентифікації та авторизації на базі Duende IdentityServer та ASP.NET Core Identity; видача токенів доступу для клієнта та API. |
| `EventNotifier.Client`           | Клієнтський вебзастосунок (Angular SPA), що надає інтерфейс для перегляду подій, підписки, особистого кабінету та адмін-панелі. |
| `ScheduleReminders`              | Проєкт Azure Functions для планування та перепланування нагадувань: містить функції `ScheduleReminders` (Timer Trigger) і `RescheduleReminders` (Queue Trigger)
| `SendEmailWorker`                | Проєкт Azure Functions з функцією `SendEmailWorker` (Queue Trigger), яка зчитує повідомлення з черги `email-reminders` та надсилає email-нагадування через `SmtpClient`. |
| `EventNotifier.AppHost`          | Оркестратор .NET Aspire (AppHost): запускає сервіси рішення, конфігурує підключення до Azure, збирає телеметрію та надає єдину точку керування під час розробки. |
| `EventNotifier.ServiceDefaults`  | Спільні налаштування для сервісів (logging, OpenTelemetry, підключення до Azure, політики повторних спроб), які підключаються до `EventNotifier` та інших проєктів. |
| `docker-compose`                 | Конфігурація `docker-compose` для локального запуску контейнерів (API, IdentityServer тощо) у єдиному середовищі розробки. |


---

## ▶️ Як запустити проєкт "з нуля"

### 1. Встановлення інструментів

- .NET 8 SDK  
- .NET Aspire (pre-installed with .NET 8 workloads)  
- Node.js (для фронтенду Angular)  
- Angular CLI 14+  
- Docker Desktop (для запуску контейнерів та docker-compose)  
- Azure CLI (для деплою у хмару - опціонально)  
- SQL Server Management Studio або Azure Data Studio (для роботи з базою даних)

### 2. Клонування репозиторію

```bash
git clone https://github.com/NataliaHerus/Event-Notifier.git
cd Event-Notifier
```

### 3. Встановлення залежностей

```bash
# Backend (EventNotifier API + оркестрація Aspire)
cd EventNotifier
dotnet restore

cd ../EventNotifier.IdentityServer
dotnet restore

cd ../EventNotifier.AppHost
dotnet restore

# Frontend (Angular SPA)
cd ../EventNotifier.Client
npm install

# Serverless функції
cd ../ScheduleReminders
dotnet restore

cd ../SendEmailWorker
dotnet restore

```

### 4. Налаштування конфігураційних файлів

#### Для backend:

```
// EventNotifier/appsettings.json
{
  "ApplicationSettings": {
    "Secret": "SuperSecretKeyForJwtOrAuth",
    "IdentityUrl": "https://localhost:7010/",
    "ClientUrl": "http://localhost:4200"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=<SQL_SERVER_NAME>;Initial Catalog=EventNotifier;User ID=<DB_USER>;Password=<DB_PASSWORD>;Encrypt=False;TrustServerCertificate=True;",
    "AzureBlobConnectionString": "<AZURE_BLOB_STORAGE_CONNECTION_STRING>"
  },
  "AzureWebJobsStorage": "<Azure_Storage_Connection_String>",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```
```
// EventNotifier.IdentityServer/appsettings.json
{
  "ApplicationSettings": {
    "Secret": "Super_Long_Secret_Key_Change_Me_123456",
    "ClientUrl": "http://localhost:4200/",
    "EventNotifierUrl": "https://localhost:7179/",
    "ResetPasswordPath": "#/auth/reset-password"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=<LOCAL_OR_AZURE_SQL>;Database=EventNotifier.IdentityServer;User ID=<DB_USER>;Password=<DB_PASSWORD>;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

```
// ScheduleReminders/local.settings.json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<Azure_Storage_Connection_String>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "SqlConnectionStringEventNotifier": "Server=<SQL_SERVER>;Database=EventNotifier;User Id=<DB_USER>;Password=<DB_PASS>;TrustServerCertificate=True;",
користувачів
    "SqlConnectionStringIdentityServer": "Server=<SQL_SERVER>;Database=EventNotifier.IdentityServer;User Id=<DB_USER>;Password=<DB_PASS>;TrustServerCertificate=True;"
  }
}

```

```
// SendEmailWorker/local.settings.json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<Azure_Storage_Connection_String>",   // черга email-reminders
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",

    "SmtpUser": "<smtp_account_email>",
    "SmtpPassword": "<smtp_app_password>",
і
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": "587"
  }
}


```


```
// docker-compose - docker-compose.override.yml
version: '3.4'

services:
  eventnotifier:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - identityUrl=https://localhost:7179/
      - ConnectionStrings__DefaultConnection=Data Source=host.docker.internal;Initial Catalog=EventNotifier;User ID=<DB_USER>;Password=<DB_PASSWORD>;Encrypt=False;TrustServerCertificate=True;
    ports:
      - "7009:80"
      - "7010:443"
    volumes:
      - ${APPDATA}/Microsoft/UserSecrets:/root/.microsoft/usersecrets:ro
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro

  eventnotifier.identityserver:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=https://+:443;http://+:80
      - ConnectionStrings__DefaultConnection=Data Source=host.docker.internal;Initial Catalog=EventNotifier.IdentityServer;User ID=<DB_USER>;Password=<DB_PASSWORD>;Encrypt=False;TrustServerCertificate=True;
    ports:
      - "7178:80"
      - "7179:443"
    volumes:
      - ${APPDATA}/Microsoft/UserSecrets:/root/.microsoft/usersecrets:ro
      - ${APPDATA}/ASP.NET/Https:/root/.aspnet/https:ro

```

#### Для frontend:

```
// EventNotifier.Client\src\environments\environment.ts

export const environment = {
  production: false,
  apiUrl: 'https://localhost:7179/',     // Backend API (EventNotifier)
  identityUrl: 'https://localhost:7010/' // IdentityServer (автентифікація/логін/реєстрація)
};


```

### 5. Запуск

```bash
# ▶️ Запуск Backend API (EventNotifier)
cd EventNotifier
dotnet run

# ▶️ Запуск IdentityServer
cd ../EventNotifier.IdentityServer
dotnet run

# ▶️ Запуск Angular Client
cd ../EventNotifier.Client
npm install
ng serve -o

# Планування/перепланування нагадувань
cd ../ScheduleReminders
func start

# Обробка надсилання email
cd ../SendEmailWorker
func start

🔥 Повний запуск через .NET Aspire (рекомендовано для розробки)
cd ../EventNotifier.AppHost
dotnet run

🐳 Альтернативний запуск через Docker
docker-compose up -d

```

---



## 🖱️ Інструкція для користувача

1. **Головна сторінка (до авторизації)** — доступний опис застосунку та мінімальне меню
   - `🔐 Увійти` – авторизація існуючого користувача
   - `📝 Зареєструватися` –  створення нового облікового запису

2. **Авторизація / Реєстрація**
   - `📧 Email + 🔑 Пароль` –  стандартний спосіб входу
   - `⚠ Повідомлення про помилку` –  якщо дані введено некоректно
   - `📝 Створити акаунт` – реєстрація з перевіркою правильності введених даних

3. **Скидання пароля**
   - `🔁 Забули пароль?` –  форма відновлення доступу
   - `📨 Отримання листа` –  інструкції для відновлення надходять на email
   - `🔒 Новий пароль` –  після встановлення користувач повертається до входу

4. **Після входу в систему**
   - `📁 Усі події` –  повний список заходів
   - `⏳ Найближчі події` –  події, які відбудуться незабаром
   - `👤 Особистий кабінет` –  профіль та збережені події
   - `🚪 Вийти` –  завершення сесії користувача

5. **Перегляд усіх подій**
   - `🔍 Пошук` –  за назвою
   - `🧭 Фільтри` –  за будь-якими параметрами
   - `📄 Пагінація` –  перехід між сторінками подій

6. **Перегляд найближчих подій**
   - `📄 Режим списку` –  компактний перегляд
   - `🖼 Режим карток` –  розширений перегляд
   - `🔎 Фільтр за датою` –  швидкий пошук майбутніх подій

7. **Детальна сторінка події**
   - `⭐ Зберегти подію` – отримати нагадування за 7 днів, за день до події та в день події
   - `🗑 Видалити із збережених` –  якщо подія вже додана
   - `🖼 Перегляд фото` –  відкриття у збільшеному форматі

8. **Особистий кабінет**
   - `👤 Перегляд особистої інформації`
   - `✏ Редагування профілю`
   - `⭐ Перегляд збережених подій`

9. **Панель адміністратора**
   - `➕ Створити подію`
   - `✏ Редагувати подію`
   - `🗑 Видалити одну або кілька`
   - `👥 Користувачі` –  перегляд, редагування, видалення
   - `📨 Створити акаунт` –  запрошення на email для встановлення пароля

---

## 📷 Приклади / скриншоти

### 🏠 Головна сторінка
![](/screenshots/home.png)

### 🔐 Авторизація
![](/screenshots/login.png)

### 📝 Реєстрація
![](/screenshots/registration.png)

### 🔑 Відновлення паролю
![](/screenshots/forgotPass.png)

---

### 📅 Усі події
![](/screenshots/allEvents.png)

### ⏳ Найближчі події — список
![](/screenshots/closestEventsList.png)

### 🟦 Найближчі події — картки
![](/screenshots/closestEventsCards.png)

### 📄 Детальна інформація про подію
![](/screenshots/EventInfo.png)

---

### 👤 Особистий кабінет
![](/screenshots/personalOffice.png)

### ✏️ Редагування профілю
![](/screenshots/editPersonalInfo.png)

---

### 🛠 Адмін-панель — події
![](/screenshots/manageEvents.png)

### ➕ Створення події
![](/screenshots/addEvent.png)

### 👥 Адмін-панель — користувачі
![](/screenshots/manageUsers.png)

### ➕ Додавання користувача
![](/screenshots/addUser.png)

---

### ✉️ Email-сповіщення
![](/screenshots/notification.png)

---

## 🧪 Проблеми і рішення

| Проблема                                       | Причина                                                               | Рішення                                                                                          |
| ---------------------------------------------- | --------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| ❗ API повертає *401 Unauthorized*              | Angular не отримує валідний access token                              | Перевірити URL IdentityServer у `environment.ts` + налаштування CORS у API                       |
| ❗ Email-нагадування не надходять               | Queue пуста або неправильний SMTP                                     | Перевірити налаштування `SmtpUser`, `SmtpPassword`, існування черги `email-reminders`            |
| ⏳ Нагадування не запускаються за розкладом     | TimerTrigger не активний або недоступне з'єднання з DB                | Перевірити Azure Functions лог `ScheduleReminders` та `SqlConnectionStringEventNotifier`         |
| 📭 Повідомлення зависають у черзі              | SendEmailWorker не читає Queue                                        | Переконатися, що функція запущена та має доступ до `AzureWebJobsStorage`                         |
| 🔐 Помилки логіну / токени не видаються        | IdentityServer не бачить базу або клієнтські конфігурації неправильні | Перевірити connection string + налаштування `ClientUrl`, `EventNotifierUrl`                      |
| 🧩 Подія змінена, але нагадування не оновились | QueueTrigger не обробляє `EventUpdatedMessage`                        | Перевірити, чи створюється повідомлення в черзі `events-updates` та чи працює Reschedule-функція |
| 🚫 Деплой в Azure — API не бачить SQL          | Відсутні firewall-настройки або wrong connection string               | Дозволити доступ по IP або увімкнути Private Endpoint                                            |
| 📦 Aspire не запускає всі сервіси              | Один із сервісів не стартує локально                                  | Перевірити логи та оновити пакет `Aspire.ServiceDefaults`                                        |


---

## 🧾 Використані джерела / література

📘 Документація та офіційні ресурси

- .NET 8 – офіційна документація Microsoft
- ASP.NET Web API – Microsoft Docs
- Duende IdentityServer – Official Documentation
- Angular – офіційна документація
- Azure Functions – Triggers & Bindings Docs
- Azure Container Apps – офіційний гайд
- Azure SQL Serverless – Microsoft Documentation
- Azure Blob Storage – Documentation
- Azure Queue Storage – Documentation
- .NET Aspire – Telemetry & Orchestration Docs

---
