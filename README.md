# Yap

A full-stack social network inspired by Twitter/X. Users create short posts (yaps), follow others, like, comment, bookmark, re-yap, use hashtags and mentions, upload media, receive notifications, report content, block users, and send direct messages.

The migration target in this repo is Azure Static Web Apps + Azure Container Apps + Azure Database for PostgreSQL + Azure Blob Storage.

## Monorepo changed to multirepo

https://github.com/fabio-jun/yap-api <br>
https://github.com/fabio-jun/yap-client

## Deploy link

https://mango-bush-0c28e7b0f.7.azurestaticapps.net/

## Tech Stack

**Backend:** ASP.NET Core 10, Entity Framework Core, PostgreSQL, Redis cache, Azure Blob Storage, Swagger/OpenAPI, xUnit

**Frontend:** React 19, TypeScript, Vite, Tailwind CSS v4, daisyUI v5

**Infrastructure:** Docker Compose, Azure Static Web Apps, Azure Container Apps, Azure Database for PostgreSQL, Azure Blob Storage

## Architecture

The backend follows Clean Architecture with four layers:

```
Blog.API           -- Controllers, middleware, dependency injection
Blog.Application   -- Services, DTOs, business logic
Blog.Domain        -- Entities, repository interfaces
Blog.Infrastructure -- EF Core, repositories, migrations, seeder
```


## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 10 SDK (for local development without Docker)
- Node.js 20+ and pnpm (for frontend development)

### Run with Docker

Create a local `.env` file in the repository root with your JWT signing key and local Postgres password:


```bash
docker compose up --build
```

This starts four containers:
- **PostgreSQL** on port 5432
- **Redis** on port 6379
- **API** on port 8080
- **Client** on port 3000 (nginx reverse proxy)

Docker Compose uses `Redis__Connection=redis:6379` and `Storage__Provider=Local`.

The database is automatically migrated and seeded with fake data (10 users, 50 posts, follows, likes, comments). Default password for all seeded users: `123456`.

Swagger is enabled in Docker and is available at `http://localhost:8080/swagger`.


### Run tests

```bash
cd yap-api
dotnet test tests/Blog.Tests
```

90 unit tests covering posts, likes, follows, users, comments, tags, reposts, reports, blocks, notifications, cache keys, and Swagger metadata (xUnit + NSubstitute).

## Project Structure

```
yap-api/
  src/
    Blog.API/            Controllers, Program.cs, middleware
    Blog.Application/    Services, DTOs, interfaces
    Blog.Domain/         Entities, repository interfaces
    Blog.Infrastructure/ DbContext, repositories, configurations, migrations, seeder
  tests/
    Blog.Tests/          Unit tests

yap-client/
  src/
    api/                 Axios API calls (auth, posts, comments, likes, follows, bookmarks, messages, notifications, reposts, reports, blocks, upload, users)
    components/          Navbar, YapCard, CreateYap, SuggestedUsers, ProfileMedia, notifications, reports, modals, etc.
    contexts/            AuthContext (JWT state management)
    pages/               HomePage, PostPage, ProfilePage, SearchPage, BookmarksPage, MessagesPage, NotificationsPage, AdminReportsPage, etc.
    types.ts             TypeScript interfaces
```

## Environment Variables

### Backend

| Variable | Description |
|----------|-------------|
| ConnectionStrings__DefaultConnection | PostgreSQL connection string |
| Jwt__Key | JWT signing key |
| Jwt__Issuer | JWT issuer |
| Jwt__Audience | JWT audience |
| Redis__Connection | Redis connection string used by the API cache |
| Storage__Provider | Storage backend selector. Azure target uses `AzureBlob` |
| AzureBlob__ConnectionString | Azure Blob Storage connection string |
| AzureBlob__ContainerName | Public blob container name for uploaded media |
| Cors__AllowedOrigins__0 | Allowed CORS origin |
| Swagger__Enabled | Enables Swagger UI outside Development when set to `true` |

### Frontend

| Variable | Description |
|----------|-------------|
| VITE_API_URL | Backend API base URL |

## Azure Target

- **Frontend:** Azure Static Web Apps
- **API:** Azure Container Apps
- **Database:** Azure Database for PostgreSQL Flexible Server
- **Media:** Azure Blob Storage
- **Cache:** Redis Cloud via Azure Marketplace

See [docs/azure-migration.md](C:\Users\fabio\Desktop\Code\Yap\docs\azure-migration.md) for the migration flow and Azure resource checklist.

## License

This project was built for learning and portfolio purposes.
