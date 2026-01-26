using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleBlockTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<double>(type: "double precision", nullable: false),
                    AssignmentId = table.Column<int>(type: "integer", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    BlockType = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleBlocks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyCapacities_UserId_DayIndex",
                table: "DailyCapacities",
                columns: new[] { "UserId", "DayIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseWeights_UserId_CourseId",
                table: "CourseWeights",
                columns: new[] { "UserId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleBlocks_ScheduleId",
                table: "ScheduleBlocks",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleBlocks");

            migrationBuilder.DropIndex(
                name: "IX_DailyCapacities_UserId_DayIndex",
                table: "DailyCapacities");

            migrationBuilder.DropIndex(
                name: "IX_CourseWeights_UserId_CourseId",
                table: "CourseWeights");
        }
    }
}
