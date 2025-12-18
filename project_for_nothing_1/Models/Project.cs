using System.ComponentModel.DataAnnotations;

namespace project_for_nothing_1.Models
{
    public class Project
    {

        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = "";

        [StringLength (2000)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }
}
