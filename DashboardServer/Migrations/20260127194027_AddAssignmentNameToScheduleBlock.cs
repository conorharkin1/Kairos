using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentNameToScheduleBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignmentName",
                table: "ScheduleBlocks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignmentName",
                table: "ScheduleBlocks");
        }
    }
}
