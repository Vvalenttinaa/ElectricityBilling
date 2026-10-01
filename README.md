### Database Migrations

Entity Framework Core migrations are applied automatically when the API starts.

When running the application using Docker:

```bash
docker compose up --build
```

no manual migration command is required. The API automatically applies all pending migrations using `Database.MigrateAsync()` and then seeds the initial application data.

To recreate the database from scratch, remove the PostgreSQL Docker volume:

```bash
docker compose down -v
docker compose up --build
```

For local development without Docker, migrations can also be applied manually:

```bash
dotnet ef database update \
  --project ElectricityBilling.Infrastructure \
  --startup-project ElectricityBilling.API
```