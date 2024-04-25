using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LectureNumber",
                table: "Sections",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "LectureVideos",
                newName: "PathOrContainer");

            migrationBuilder.AddColumn<bool>(
                name: "IsDrafted",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDrafted",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Sections",
                newName: "LectureNumber");

            migrationBuilder.RenameColumn(
                name: "PathOrContainer",
                table: "LectureVideos",
                newName: "Path");
        }
    }
}
