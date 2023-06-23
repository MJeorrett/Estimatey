using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectIdToLoggedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "LoggedTime",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LoggedTime_ProjectId",
                table: "LoggedTime",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoggedTime_Project_ProjectId",
                table: "LoggedTime",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoggedTime_Project_ProjectId",
                table: "LoggedTime");

            migrationBuilder.DropIndex(
                name: "IX_LoggedTime_ProjectId",
                table: "LoggedTime");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "LoggedTime");
        }
    }
}
