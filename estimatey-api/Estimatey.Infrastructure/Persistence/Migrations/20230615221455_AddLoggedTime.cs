using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLoggedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LoggedTimeHasBeenSyncedUntil",
                table: "Project",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoggedTime",
                columns: table => new
                {
                    LoggedTimeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FloatId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FloatPersonId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Hours = table.Column<double>(type: "float", nullable: false),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    LockedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggedTime", x => x.LoggedTimeId);
                    table.ForeignKey(
                        name: "FK_LoggedTime_FloatPerson_FloatPersonId",
                        column: x => x.FloatPersonId,
                        principalTable: "FloatPerson",
                        principalColumn: "FloatPersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoggedTime_FloatPersonId",
                table: "LoggedTime",
                column: "FloatPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoggedTime");

            migrationBuilder.DropColumn(
                name: "LoggedTimeHasBeenSyncedUntil",
                table: "Project");
        }
    }
}
