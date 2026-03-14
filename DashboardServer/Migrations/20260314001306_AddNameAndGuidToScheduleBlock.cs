using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class AddNameAndGuidToScheduleBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlockId",
                table: "ScheduleBlocks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ScheduleBlocks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockId",
                table: "ScheduleBlocks");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ScheduleBlocks");
        }
    }
}
