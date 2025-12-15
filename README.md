# Staffinity Personal Microservice

Microservice responsible for managing employee personal information, vacation requests, and notifications.

## 🏗 Architecture
This project follows a **Hexagonal Architecture (Clean Architecture)** approach, modularized to enable parallel development.

### Project Structure (`src/`)
- **Api**: Entry point (Controllers). Configuration and Dependency Injection.
- **Application**: Use Cases (Pure Business Logic) and DTOs.
- **Domain**: Core Entities, Value Objects, and Ports (Interfaces).
- **Infrastructure**: Port Implementations (Database, External Services).

### Modules
The codebase is organized vertically by feature to minimize merge conflicts:
- `Employees` (Employee management)
- `Vacations` (Requests and approvals)
- `Notifications` (Alerts and emails)

---

## Environment configuration

1. Copy `.env.example` to `.env`, fill in the placeholders, and keep the real values local.
2. `DotNetEnv` reads `.env` when the ASP.NET host starts, so the API can load credentials without checking them into Git.
3. You can also export the required values directly:
   ```powershell
   $env:ConnectionStrings__Default = "Host=localhost;Port=5432;Database=staffinity_personal;Username=youruser;Password=yourpassword;"
   $env:Jwt__Secret = "your_jwt_secret"
   dotnet run --project src/Staffinity.Personal.Api
   ```
4. On Unix-like shells use `export` instead of PowerShell syntax, and never commit secrets to the repository.

## Running the API securely

1. Provide the database and JWT configuration through environment variables before starting the host.
2. Run `dotnet run --project src/Staffinity.Personal.Api` from the repository root.
3. Swagger UI at `/swagger` now exposes bearer authentication; use a valid JWT before calling protected endpoints.
4. For Docker Compose, override the same environment variables instead of hardcoding secrets.

## 🚀 How to Run Locally

### Prerequisites
1.  **.NET 10 SDK** (or higher).
2.  **Docker** (Required to run the database).
3.  **JetBrains Rider** (Recommended) or VS Code.

### Step 1: Start Database (PostgreSQL)
If you don't have a local Postgres instance, run this command in your terminal to start a temporary one using Docker:

```bash
docker run --name staffinity-postgres -e POSTGRES_PASSWORD=your_secure_password -e POSTGRES_DB=StaffinityPersonalDb -p 5432:5432 -d postgres

## Automation example (n8n)
If you want an n8n workflow to expose the most sold product (for dashboards or alerts), you can set it up like this:

1) **Webhook (POST /best-seller)**: entry point; optionally validate a shared secret in the headers.
2) **HTTP Request**: call your products API (or DB) to fetch sales data, e.g. `GET https://your.api/products/sales`.
3) **Function**: filter the array to the product with the highest `totalSold`.
   ```javascript
   // Items from HTTP node in items[0].json.products
   const products = items[0].json.products ?? [];
   if (!products.length) return [{ json: { message: 'No products' } }];

   const top = products.reduce((best, current) =>
     current.totalSold > (best?.totalSold ?? -1) ? current : best, null);

   return [{ json: { bestSeller: top } }];
   ```
4) **Respond to Webhook**: return the `bestSeller` JSON to the caller (or send it to Slack/Email if preferred).

Publish the webhook URL from your n8n instance and secure it (basic auth, secret header, or IP allowlist).***
