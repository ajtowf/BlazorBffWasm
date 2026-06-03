# Backend Application

This is the backend application for the Blazor BFF sample that implements the todo functionality with Entity Framework Core database persistence.

## Features

- Todo API endpoints with full CRUD operations
- Entity Framework Core database integration with SQL Server LocalDB
- Database migrations and seeding
- BFF pattern integration

## Database Setup

The application uses SQL Server LocalDB for persistent storage. The database connection string is configured in `appsettings.json`.

## API Endpoints

- `GET /api/todo` - Get all todos
- `GET /api/todo/{id}` - Get a specific todo
- `POST /api/todo` - Create a new todo
- `PUT /api/todo/{id}` - Update a todo
- `DELETE /api/todo/{id}` - Delete a todo

## Migration

The initial database migration was created using:
```
dotnet ef migrations add InitialCreate
```

The database is automatically created and seeded when the application starts.