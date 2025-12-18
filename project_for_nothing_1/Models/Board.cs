
using System.ComponentModel.DataAnnotations;

namespace project_for_nothing_1.Models
{
    public class Board
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = default!;

        [Required]
        [StringLength(120)]
        public string Name { get; set; } = "Board";

        public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
