using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIterationToWorkItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Iteration",
                table: "UserStory",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Iteration",
                table: "Ticket",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Iteration",
                table: "Feature",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iteration",
                table: "UserStory");

            migrationBuilder.DropColumn(
                name: "Iteration",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Iteration",
                table: "Feature");
        }
    }
}
