using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class StudentCLassYear2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeId1",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_GradeId1",
                table: "Students",
                column: "GradeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Grades_GradeId1",
                table: "Students",
                column: "GradeId1",
                principalTable: "Grades",
                principalColumn: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Grades_GradeId1",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_GradeId1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GradeId1",
                table: "Students");
        }
    }
}
