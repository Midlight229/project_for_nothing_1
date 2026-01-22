using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_for_nothing_1.Migrations
{
    /// <inheritdoc />
    public partial class init21012026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Assignees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Assignees");
        }
    }
}
