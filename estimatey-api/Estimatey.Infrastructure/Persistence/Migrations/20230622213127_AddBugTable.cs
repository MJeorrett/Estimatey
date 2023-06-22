using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBugTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bug",
                columns: table => new
                {
                    BugId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserStoryId = table.Column<int>(type: "int", nullable: true),
                    DevOpsId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Iteration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bug", x => x.BugId);
                    table.ForeignKey(
                        name: "FK_Bug_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bug_UserStory_UserStoryId",
                        column: x => x.UserStoryId,
                        principalTable: "UserStory",
                        principalColumn: "UserStoryId");
                });

            migrationBuilder.CreateTable(
                name: "BugTag",
                columns: table => new
                {
                    BugId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BugTag", x => new { x.BugId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BugTag_Bug_BugId",
                        column: x => x.BugId,
                        principalTable: "Bug",
                        principalColumn: "BugId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BugTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bug_DevOpsId",
                table: "Bug",
                column: "DevOpsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bug_ProjectId",
                table: "Bug",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Bug_UserStoryId",
                table: "Bug",
                column: "UserStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BugTag_TagId",
                table: "BugTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BugTag");

            migrationBuilder.DropTable(
                name: "Bug");
        }
    }
}
