# Helpdesk API

Modern .NET 8 Web API for the PROSHIKA helpdesk. This skeleton mirrors the legacy ASP.NET MVC app and will expose all existing flows (auth, tokens, attachments, comments, categories, vendor/support admin, dashboard, reports) over JWT-secured endpoints.

## Structure
- `Helpdesk.sln` — solution file.
- `Helpdesk.Api/` — ASP.NET Core API project (controllers, startup, Swagger/JWT).
- `Helpdesk.Domain/` — domain entities and common abstractions.
- `Helpdesk.Infrastructure/` — EF Core DbContext, SQL Server access, DI extensions.
- `Helpdesk.Contracts/` — request/response DTOs and validation contracts.

## Configuration
- Connection string key: `ConnectionStrings:HelpdeskDatabase`.
- JWT settings: `Jwt:Issuer`, `Jwt:Audience`, `Jwt:SigningKey`.
- Development appsettings currently point to the legacy SQL Server; move secrets to user-secrets or environment variables before production.

## Run
```bash
cd helpdeskapi
dotnet build Helpdesk.sln
dotnet run --project Helpdesk.Api/Helpdesk.Api.csproj
```
Swagger UI: `/swagger`
Health: `GET /api/health`
