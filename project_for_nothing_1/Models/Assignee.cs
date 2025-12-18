using System.ComponentModel.DataAnnotations;

namespace project_for_nothing_1.Models
{
    public class Assignee
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string FullName { get; set; } = "";
        [EmailAddress, StringLength(200)]
        public string? Email {  get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
