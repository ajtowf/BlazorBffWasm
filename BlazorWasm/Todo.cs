using System.ComponentModel.DataAnnotations;

namespace BlazorWasm
{
    public class Todo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}