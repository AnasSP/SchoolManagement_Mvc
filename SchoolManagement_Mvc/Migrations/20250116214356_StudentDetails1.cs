using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class StudentDetails1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GradeValue",
                table: "Enrolls",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Enrolls_SubjectId",
                table: "Enrolls",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolls_Subjects_SubjectId",
                table: "Enrolls",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrolls_Subjects_SubjectId",
                table: "Enrolls");

            migrationBuilder.DropIndex(
                name: "IX_Enrolls_SubjectId",
                table: "Enrolls");

            migrationBuilder.DropColumn(
                name: "GradeValue",
                table: "Enrolls");
        }
    }
}
