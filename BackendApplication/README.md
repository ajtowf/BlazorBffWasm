# BackendApplication

This is the backend application for the Blazor BFF (Backend for Frontend) sample application.

## Features

- Todo API endpoints for managing todos
- In-memory storage for todos (as requested)
- Authentication integration with Duende BFF
- SignalR hub for chat functionality

## Todo API Endpoints

The backend exposes the following API endpoints for todo management:

- `GET /api/todo` - Get all todos
- `POST /api/todo` - Create a new todo
- `PUT /api/todo/{id}` - Update an existing todo
- `DELETE /api/todo/{id}` - Delete a todo

## Data Persistence

Todos are stored in-memory within the backend application. This is for demonstration purposes only and will be lost when the application restarts.

## Authentication

The backend integrates with Duende BFF for authentication. The BFF handles authentication tokens and forwards them to the backend when needed.

## Project Structure

- `Controllers/TodoController.cs` - API endpoints for todo management
- `Todo.cs` - Todo model definition
- `Program.cs` - Application startup configuration
- `Startup.cs` - Services configuration