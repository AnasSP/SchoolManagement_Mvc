using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class StudentDetails2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrolls_Subjects_SubjectId",
                table: "Enrolls");

            migrationBuilder.DropIndex(
                name: "IX_Enrolls_SubjectId",
                table: "Enrolls");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
