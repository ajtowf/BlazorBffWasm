# Blazor BFF WebAssembly Application

This is a Blazor WebAssembly application that demonstrates a BFF (Backend for Frontend) pattern with authentication.

## Features

- Authentication with Duende BFF
- Home page that displays user claims when signed in
- Todo list functionality (data stored in-memory on the backend)
- Responsive UI with Bootstrap styling

## Todo List Functionality

The todo list feature:
- Is only visible when a user is signed in
- Allows adding new todos
- Supports marking todos as complete/incomplete
- Allows deleting todos
- Data is stored in-memory on the backend (for demonstration purposes)

## Project Structure

- `Pages/Home.razor` - Main home page with todo list
- `Components/TodoList.razor` - The todo list component
- `Todo.cs` - Todo model
- `TodoService.cs` - Service for managing todos (in-memory storage)
- `wwwroot/css/todo.css` - Styling for the todo list

## Authentication

The application uses Duende BFF for authentication. The home page shows user claims when signed in.

## Building and Running

To build and run the application:

```bash
dotnet run
```

The application will start and be available at `https://localhost:5001` (or `http://localhost:5000`).