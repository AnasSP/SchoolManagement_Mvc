using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class StudentSpecialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SessionId",
                table: "Students",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Sessions_SessionId",
                table: "Students",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Sessions_SessionId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_SessionId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Students");
        }
    }
}
