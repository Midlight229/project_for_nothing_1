using Microsoft.EntityFrameworkCore;
using project_for_nothing_1.Models;

namespace project_for_nothing_1.CodeData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<BoardColumn> BoardColumns => Set<BoardColumn>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<Assignee> Assignees => Set<Assignee>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            // Project -> Board (1:M)
            b.Entity<Board>()
                .HasOne(x=>x.Project)
                .WithMany(p=>p.Boards)
                .HasForeignKey(x=>x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Board -> Column (1:M)
            b.Entity<BoardColumn>()
                .HasOne(x => x.Board)
                .WithMany(p => p.Columns)
                .HasForeignKey(x => x.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Board -> Task (1:M)
            b.Entity<TaskItem>()
                .HasOne(x => x.Board)
                .WithMany(p => p.Tasks)
                .HasForeignKey(x => x.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            // Column -> Task (1:M)
            b.Entity<TaskItem>()
                .HasOne(x => x.Column)
                .WithMany(p => p.Tasks)
                .HasForeignKey(x => x.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            // Assignee -> Task (1:M)
            b.Entity<TaskItem>()
                .HasOne(x => x.Assignee)
                .WithMany(p => p.Tasks)
                .HasForeignKey(x => x.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Unique names of columns in Board
            b.Entity<BoardColumn>()
                .HasIndex(x => new { x.BoardId, x.Name })
                .IsUnique();

            // Порядок колонок и задач
            b.Entity<BoardColumn>().Property(p => p.Order).HasDefaultValue(0);
            b.Entity<TaskItem>().Property(p => p.Order).HasDefaultValue(0);
        }
    }
}
