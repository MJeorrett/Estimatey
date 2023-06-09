using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLinksContinuationTokenToProjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevOpsContinuationToken",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "LinksContinuationToken",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkItemsContinuationToken",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinksContinuationToken",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "WorkItemsContinuationToken",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "DevOpsContinuationToken",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
