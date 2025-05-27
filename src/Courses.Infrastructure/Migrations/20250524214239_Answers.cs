using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Answers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answers",
                table: "Question",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers",
                table: "Question");
        }
    }
}
