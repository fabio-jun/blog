# Yap

A full-stack social network inspired by Twitter/X. Users create short posts (yaps), follow others, like, comment, bookmark, re-yap, use hashtags and mentions, upload media, receive notifications, report content, block users, and send direct messages.

Deployed on Vercel (frontend) + Render (API) + Neon (database).

## Monorepo changed to multirepo

https://github.com/fabio-jun/yap-api <br>
https://github.com/fabio-jun/yap-client

## Deploy link

https://yap-client.vercel.app/

## Tech Stack

**Backend:** ASP.NET Core 10, Entity Framework Core, PostgreSQL, Redis, Cloudinary, Swagger/OpenAPI, xUnit

**Frontend:** React 19, TypeScript, Vite, Tailwind CSS v4, daisyUI v5

**Infrastructure:** Docker Compose, GitHub Actions CI/CD, Render, Vercel, Neon

## Architecture

The backend follows Clean Architecture with four layers:

```
Blog.API           -- Controllers, middleware, dependency injection
Blog.Application   -- Services, DTOs, business logic
Blog.Domain        -- Entities, repository interfaces
Blog.Infrastructure -- EF Core, repositories, migrations, seeder
```

Recent backend modules include notifications, re-yaps, reports, blocks, direct messages, bookmarks, follows, likes, comments, tags, users, uploads, and Redis cache support.

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 10 SDK (for local development without Docker)
- Node.js 20+ and pnpm (for frontend development)

### Run with Docker

Create a local `.env` file in the repository root with your rotated Cloudinary URL, a long random JWT signing key, and a local Postgres password:


```bash
docker compose up --build
```

This starts four containers:
- **PostgreSQL** on port 5432
- **Redis** on port 6379
- **API** on port 8080
- **Client** on port 3000 (nginx reverse proxy)

The database is automatically migrated and seeded with fake data (10 users, 50 posts, follows, likes, comments). Default password for all seeded users: `123456`.

Swagger is enabled in Docker and is available at `http://localhost:8080/swagger`.


### Run tests

```bash
cd yap-api
dotnet test tests/Blog.Tests
```

82 unit tests covering posts, likes, follows, users, comments, tags, reposts, reports, blocks, notifications, and cache keys (xUnit + NSubstitute).

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
| Cloudinary__Url | Cloudinary URL for media uploads |
| Redis__Connection / REDIS__CONNECTION | Redis connection string |
| Cors__AllowedOrigins__0 | Allowed CORS origin |
| Swagger__Enabled | Enables Swagger UI outside Development when set to `true` |

### Frontend

| Variable | Description |
|----------|-------------|
| VITE_API_URL | Backend API base URL |

## CI/CD

GitHub Actions runs on every push to `master`:
1. Restore, build, and test the backend
2. On success, triggers a deploy hook to Render

The frontend auto-deploys on Vercel when connected to the repository.

Render also expects a Redis connection string through `REDIS__CONNECTION` when caching is enabled.

## License

This project was built for learning and portfolio purposes.
