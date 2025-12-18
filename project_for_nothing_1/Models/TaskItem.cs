using System.ComponentModel.DataAnnotations;

namespace project_for_nothing_1.Models
{

    public enum TaskPriority { Low=0,Medium=1, High=2,Critical=3}
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public int BoardId { get; set; }
        public Board Board { get; set; } = default!;

        [Required] 
        public int ColumnId { get; set; }
        public BoardColumn Column { get; set; } = default!;

        [Required, StringLength(160)]
        public string Title { get; set; } = "";

        [StringLength(5000)]
        public string? Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

        public int? AssigneeId { get; set; }
        public Assignee? Assignee { get; set; }

        public int Order { get; set; } = 0;


    }
}
