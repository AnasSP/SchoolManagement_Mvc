using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class SubjectYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_GradeId",
                table: "Subjects",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Grades_GradeId",
                table: "Subjects",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Grades_GradeId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_GradeId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Subjects");
        }
    }
}
