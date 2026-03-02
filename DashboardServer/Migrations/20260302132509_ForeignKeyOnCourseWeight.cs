using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyOnCourseWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CourseWeights_CourseId",
                table: "CourseWeights",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseWeights_Courses_CourseId",
                table: "CourseWeights",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseWeights_Courses_CourseId",
                table: "CourseWeights");

            migrationBuilder.DropIndex(
                name: "IX_CourseWeights_CourseId",
                table: "CourseWeights");
        }
    }
}
