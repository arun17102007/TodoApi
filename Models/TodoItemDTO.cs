using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItemDTO
    {
        [Required]
        [Range(0, long.MaxValue, ErrorMessage ="InValid value")]
        public long Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage ="Length should not exceed 50 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Length should not exceed 500 characters")]
        public string Description { get; set; }
        public bool IsComplete { get; set; }

    }
}
