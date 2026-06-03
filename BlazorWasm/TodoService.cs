using System.Net.Http.Json;

namespace BlazorWasm
{
    public class TodoService
    {
        private readonly HttpClient _httpClient;

        public TodoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Todo>> GetTodosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("remoteapi/todo");
                response.EnsureSuccessStatusCode();
                var todos = await response.Content.ReadFromJsonAsync<List<Todo>>();
                return todos ?? new List<Todo>();
            }
            catch (Exception)
            {
                // In a real app, you might want to handle this more gracefully
                return new List<Todo>();
            }
        }

        public async Task<Todo> AddTodoAsync(Todo todo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("remoteapi/todo", todo);
                response.EnsureSuccessStatusCode();
                var createdTodo = await response.Content.ReadFromJsonAsync<Todo>();
                return createdTodo ?? todo;
            }
            catch (Exception)
            {
                // In a real app, you might want to handle this more gracefully
                return todo;
            }
        }

        public async Task<Todo> UpdateTodoAsync(Todo todo)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"remoteapi/todo/{todo.Id}", todo);
                response.EnsureSuccessStatusCode();
                var updatedTodo = await response.Content.ReadFromJsonAsync<Todo>();
                return updatedTodo ?? todo;
            }
            catch (Exception)
            {
                // In a real app, you might want to handle this more gracefully
                return todo;
            }
        }

        public async Task DeleteTodoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"remoteapi/todo/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                // In a real app, you might want to handle this more gracefully
                throw;
            }
        }
    }
}