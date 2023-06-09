using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedColumnToWorkItemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserStory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Ticket",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Feature",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserStory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Feature");
        }
    }
}
