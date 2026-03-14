using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightingValuesToSchedulerRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CoursePreferenceWeighting",
                table: "SchedulerRules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DeadlineUrgencyWeighting",
                table: "SchedulerRules",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoursePreferenceWeighting",
                table: "SchedulerRules");

            migrationBuilder.DropColumn(
                name: "DeadlineUrgencyWeighting",
                table: "SchedulerRules");
        }
    }
}
