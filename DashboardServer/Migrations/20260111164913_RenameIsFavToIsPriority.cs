using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameIsFavToIsPriority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFavourite",
                table: "Quizzes",
                newName: "IsPriority");

            migrationBuilder.RenameColumn(
                name: "IsFavourite",
                table: "Assignments",
                newName: "IsPriority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPriority",
                table: "Quizzes",
                newName: "IsFavourite");

            migrationBuilder.RenameColumn(
                name: "IsPriority",
                table: "Assignments",
                newName: "IsFavourite");
        }
    }
}
