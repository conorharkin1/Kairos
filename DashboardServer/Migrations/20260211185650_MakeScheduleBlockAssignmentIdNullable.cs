using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class MakeScheduleBlockAssignmentIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleBlocks_Assignments_AssignmentId",
                table: "ScheduleBlocks");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleBlocks_AssignmentId",
                table: "ScheduleBlocks");

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "ScheduleBlocks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "ScheduleBlocks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBlocks_AssignmentId",
                table: "ScheduleBlocks",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleBlocks_Assignments_AssignmentId",
                table: "ScheduleBlocks",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
