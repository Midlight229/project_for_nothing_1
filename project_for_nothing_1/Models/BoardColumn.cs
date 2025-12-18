using System.ComponentModel.DataAnnotations;

namespace project_for_nothing_1.Models
{
    public class BoardColumn
    {
        public int Id { get; set; }

        [Required]
        public int BoardId { get; set; }
        public Board Board { get; set; } = default!;

        [Required, StringLength(80)]
        public string Name { get; set; } = "To Do";

        public int? WipLimit { get; set; }

        public int Order { get; set; } = 0;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
