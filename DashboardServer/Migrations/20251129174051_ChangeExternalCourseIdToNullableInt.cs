using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DashboardServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExternalCourseIdToNullableInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alter the column with explicit conversion
            migrationBuilder.Sql(@"
                ALTER TABLE ""Courses"" 
                ALTER COLUMN ""ExternalCourseId"" TYPE integer 
                USING ""ExternalCourseId""::integer
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Courses""
                ALTER COLUMN ""ExternalCourseId"" DROP NOT NULL
            ");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalAssignmentId",
                table: "Assignments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExternalCourseId",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExternalAssignmentId",
                table: "Assignments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
