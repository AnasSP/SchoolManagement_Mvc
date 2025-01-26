using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement_Mvc.Migrations
{
    /// <inheritdoc />
    public partial class SessionName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionEnd",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "SessionStart",
                table: "Sessions",
                newName: "SessionName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionName",
                table: "Sessions",
                newName: "SessionStart");

            migrationBuilder.AddColumn<string>(
                name: "SessionEnd",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
