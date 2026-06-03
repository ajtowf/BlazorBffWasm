using Microsoft.AspNetCore.Mvc;

namespace BackendApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private static readonly List<Todo> _todos = new();
        private static int _nextId = 1;

        [HttpGet]
        public IActionResult GetTodos()
        {
            return Ok(_todos);
        }

        [HttpPost]
        public IActionResult AddTodo([FromBody] Todo todo)
        {
            if (todo == null)
                return BadRequest("Todo cannot be null");

            if (string.IsNullOrWhiteSpace(todo.Title))
                return BadRequest("Todo title cannot be empty");

            todo.Id = _nextId++;
            todo.CreatedAt = DateTime.UtcNow;
            _todos.Add(todo);

            return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, [FromBody] Todo todo)
        {
            if (todo == null)
                return BadRequest("Todo cannot be null");

            var existingTodo = _todos.FirstOrDefault(t => t.Id == id);
            if (existingTodo == null)
                return NotFound($"Todo with id {id} not found");

            existingTodo.Title = todo.Title;
            existingTodo.IsCompleted = todo.IsCompleted;

            return Ok(existingTodo);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
                return NotFound($"Todo with id {id} not found");

            _todos.Remove(todo);
            return Ok();
        }
    }
}