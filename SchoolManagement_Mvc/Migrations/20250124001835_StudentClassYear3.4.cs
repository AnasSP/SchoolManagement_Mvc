using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class StudentClassYear34 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_GradeId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Grades_GradeId1",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "GradeId1",
                table: "Students",
                newName: "ClassId1");

            migrationBuilder.RenameIndex(
                name: "IX_Students_GradeId1",
                table: "Students",
                newName: "IX_Students_ClassId1");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId1",
                table: "Students",
                column: "ClassId1",
                principalTable: "Classes",
                principalColumn: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Grades_GradeId",
                table: "Students",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId1",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Grades_GradeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ClassId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "ClassId1",
                table: "Students",
                newName: "GradeId1");

            migrationBuilder.RenameIndex(
                name: "IX_Students_ClassId1",
                table: "Students",
                newName: "IX_Students_GradeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_GradeId",
                table: "Students",
                column: "GradeId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Grades_GradeId1",
                table: "Students",
                column: "GradeId1",
                principalTable: "Grades",
                principalColumn: "GradeId");
        }
    }
}
