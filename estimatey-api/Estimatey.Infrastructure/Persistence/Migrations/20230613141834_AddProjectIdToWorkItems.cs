using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectIdToWorkItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "UserStory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Feature",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserStory_ProjectId",
                table: "UserStory",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ProjectId",
                table: "Ticket",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Feature_ProjectId",
                table: "Feature",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feature_Project_ProjectId",
                table: "Feature",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Project_ProjectId",
                table: "Ticket",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStory_Project_ProjectId",
                table: "UserStory",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feature_Project_ProjectId",
                table: "Feature");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Project_ProjectId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStory_Project_ProjectId",
                table: "UserStory");

            migrationBuilder.DropIndex(
                name: "IX_UserStory_ProjectId",
                table: "UserStory");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_ProjectId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Feature_ProjectId",
                table: "Feature");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "UserStory");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Feature");
        }
    }
}
