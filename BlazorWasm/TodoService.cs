using System.Net.Http.Json;

namespace BlazorWasm
{
    public class TodoService
    {
        private readonly HttpClient _httpClient;
        private readonly List<Todo> _todos = new();
        private int _nextId = 1;

        public TodoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Todo>> GetTodosAsync()
        {
            // In a real app, this would call the backend API
            // For now, we'll return the in-memory list
            return _todos;
        }

        public async Task<Todo> AddTodoAsync(Todo todo)
        {
            // In a real app, this would call the backend API
            // For now, we'll store it in memory
            todo.Id = _nextId++;
            todo.CreatedAt = DateTime.UtcNow;
            _todos.Add(todo);
            return todo;
        }

        public async Task<Todo> UpdateTodoAsync(Todo todo)
        {
            // In a real app, this would call the backend API
            // For now, we'll update it in memory
            var existingTodo = _todos.FirstOrDefault(t => t.Id == todo.Id);
            if (existingTodo != null)
            {
                existingTodo.Title = todo.Title;
                existingTodo.IsCompleted = todo.IsCompleted;
            }
            return todo;
        }

        public async Task DeleteTodoAsync(int id)
        {
            // In a real app, this would call the backend API
            // For now, we'll remove it from memory
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _todos.Remove(todo);
            }
        }
    }
}