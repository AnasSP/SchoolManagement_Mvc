using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class StudentClassYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassName",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Students",
                newName: "GradeId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_ClassName",
                table: "Students",
                newName: "IX_Students_GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_GradeId",
                table: "Students",
                column: "GradeId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_GradeId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "Students",
                newName: "ClassName");

            migrationBuilder.RenameIndex(
                name: "IX_Students_GradeId",
                table: "Students",
                newName: "IX_Students_ClassName");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassName",
                table: "Students",
                column: "ClassName",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
