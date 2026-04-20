[![Build and Run API](https://github.com/nate-demo/AccountRegistrationApiDemo/actions/workflows/ci.yml/badge.svg)](https://github.com/nate-demo/AccountRegistrationApiDemo/actions/workflows/ci.yml)
# AccountRegistrationApiDemo

A lightweight, beginner-friendly **ASP.NET Core Web API** that uses **static JSON files** as its data source — no database, no EF Core, no authentication. Ideal for training, demos, and learning REST API fundamentals.

📘 Looking for request/response examples? See [API_DOCUMENTATION.md](API_DOCUMENTATION.md).

---

## 📁 Project Structure

```
AccountRegistrationApiDemo/
├── Controllers/                   # API controllers (Accounts, Registrations)
├── Common/
│   ├── Extensions/                # Pagination helper
│   └── Middleware/                 # Global exception handling
├── Data/
│   ├── accounts.json              # 10 sample accounts (seed data)
│   ├── registrations.json         # 25 sample registrations (seed data)
│   ├── DataSeeder.cs              # Loads JSON → in-memory store at startup
│   └── InMemoryDataStore.cs       # Thread-safe ConcurrentDictionary store
├── DTOs/
│   ├── Requests/                  # CreateAccountRequest, UpdateAccountRequest, etc.
│   └── Responses/                 # AccountResponse, RegistrationResponse, PaginatedResponse<T>
├── Mappings/                      # AutoMapper profiles
├── Models/
│   ├── Entities/                  # Account, Registration POCOs
│   └── Enums/                     # AccountStatus, RegistrationStatus
├── Services/                      # Business logic (IAccountService, IRegistrationService)
├── Validators/                    # FluentValidation validators
├── Program.cs                     # Application entry point & DI configuration
└── README.md
```

---

## 🚀 How to Run

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (or later)

### Steps

```bash
# Clone or navigate to the project folder
cd AccountRegistrationApiDemo

# Restore packages
dotnet restore

# Run the application
dotnet run
```

The API will start on **http://localhost:5000** (and **https://localhost:5001** if HTTPS is configured).

### Open Swagger UI

Navigate to:

```
http://localhost:5000/swagger
```

You'll see an interactive UI where you can test every endpoint directly in the browser.

---

## 📦 Where the Data Lives

| File | Location | Contents |
|------|----------|----------|
| `accounts.json` | `Data/accounts.json` | 10 sample accounts with diverse names, emails, and statuses |
| `registrations.json` | `Data/registrations.json` | 25 registrations (2–3 per account) for various courses/events |

### ⚠️ Data Resets on Restart

Both files are loaded into **in-memory collections** at startup. Any changes you make via the API (create, update, delete) are **lost when the app restarts**. The original JSON files are never modified — they serve as the seed data.

---

## 🔗 API Endpoints

### Accounts

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/accounts` | List accounts (paginated, searchable by name/email) |
| `GET` | `/api/accounts/{id}` | Get account by Id (includes registrations) |
| `POST` | `/api/accounts` | Create a new account |
| `PUT` | `/api/accounts/{id}` | Update an account (full replacement) |
| `DELETE` | `/api/accounts/{id}` | Delete an account + its registrations |
| `GET` | `/api/accounts/{id}/registrations` | Get all registrations for an account |

### Registrations

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/registrations` | List all registrations (paginated, filterable by accountId & status) |
| `POST` | `/api/registrations` | Create a registration for an existing account |

### Pagination

All list endpoints support `?page=1&pageSize=10` query parameters. Responses include:

```json
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "data": [ ... ]
}
```

---

## 🧪 Testing the API

### Option 1: VS Code REST Client (Recommended for Developers)

We provide comprehensive `.http` test files that can be executed directly in VS Code, acting like Postman tests. This is the fastest way to test all endpoints!

**Prerequisites:**
- Install the [REST Client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) for VS Code

**Quick Start:**
1. Start the API with `dotnet run`
2. Open `Tests/AccountsController.http` or `Tests/RegistrationsController.http` in VS Code
3. Click "Send Request" above any HTTP request
4. View the response in the split pane

**Features:**
- ✅ All endpoints covered (Accounts & Registrations)
- ✅ Happy path tests
- ✅ Validation and error handling tests
- ✅ Variables for easy ID management
- ✅ Complete workflow tests
- ✅ Version control friendly (plain text files)

📖 See [Tests/README.md](Tests/README.md) for detailed instructions and tips.

### Option 2: Swagger UI (Interactive Browser Testing)

1. Run the app with `dotnet run`
2. Open `http://localhost:5000/swagger`
3. Click on any endpoint → **Try it out** → Fill in parameters → **Execute**
4. Observe the response body, status code, and headers

### Quick Test Flow

1. **GET** `/api/accounts` — see all 10 seeded accounts
2. **GET** `/api/accounts/{id}` — pick an Id from step 1 to see details + registrations
3. **POST** `/api/accounts` — create a new account (it'll appear in the list)
4. **POST** `/api/registrations` — register the new account for an event
5. **DELETE** `/api/accounts/{id}` — remove the account (its registrations are also deleted)
6. **Restart the app** — everything resets to the original JSON data

---

## 🛠 Tech Stack

| Component | Package/Tech |
|-----------|-------------|
| Framework | ASP.NET Core 9 (Controller-based Web API) |
| Object Mapping | [AutoMapper](https://automapper.org/) v16 |
| Validation | [FluentValidation](https://docs.fluentvalidation.net/) v11 |
| API Docs | [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) (Swagger UI) |
| Data Store | In-memory `ConcurrentDictionary` seeded from JSON |

---

## 📝 Notes for Trainers

- **No database setup required** — just `dotnet run` and go
- **No authentication** — endpoints are open for easy exploration
- **Thread-safe** — uses `ConcurrentDictionary` so you can demo concurrent requests
- **Validation** — all create/update endpoints validate input and return structured error responses
- **AutoMapper licensing** — v15+ requires a license key; community licenses are free for small orgs (< $5M revenue). See [automapper.io](https://automapper.io) for details. The app works without a key but will log a warning.
