using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAndChangedDatesToWorkItemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedDate",
                table: "UserStory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserStory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedDate",
                table: "Ticket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Ticket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedDate",
                table: "Feature",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Feature",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedDate",
                table: "UserStory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserStory");

            migrationBuilder.DropColumn(
                name: "ChangedDate",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ChangedDate",
                table: "Feature");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Feature");
        }
    }
}
