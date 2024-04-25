using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Storage",
                table: "Tool");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "LectureVideos");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "LectureFile");

            migrationBuilder.DropColumn(
                name: "CourseFragmentVideoStorage",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "Tool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "LectureVideos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "LectureFile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseFragmentVideoStorage",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
