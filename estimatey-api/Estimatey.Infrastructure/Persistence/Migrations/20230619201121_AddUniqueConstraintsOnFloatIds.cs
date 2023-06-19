using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintsOnFloatIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FloatId",
                table: "LoggedTime",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_LoggedTime_FloatId",
                table: "LoggedTime",
                column: "FloatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FloatPerson_FloatId",
                table: "FloatPerson",
                column: "FloatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LoggedTime_FloatId",
                table: "LoggedTime");

            migrationBuilder.DropIndex(
                name: "IX_FloatPerson_FloatId",
                table: "FloatPerson");

            migrationBuilder.AlterColumn<string>(
                name: "FloatId",
                table: "LoggedTime",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)",
                oldMaxLength: 24);
        }
    }
}
