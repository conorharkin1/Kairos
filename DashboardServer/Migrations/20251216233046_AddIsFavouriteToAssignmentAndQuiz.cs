using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFavouriteToAssignmentAndQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "Quizzes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "Assignments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "Assignments");
        }
    }
}
