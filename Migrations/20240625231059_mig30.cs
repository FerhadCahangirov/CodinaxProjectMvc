using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Video1080p",
                table: "LectureFiles");

            migrationBuilder.DropColumn(
                name: "Video240p",
                table: "LectureFiles");

            migrationBuilder.DropColumn(
                name: "Video360p",
                table: "LectureFiles");

            migrationBuilder.DropColumn(
                name: "Video720p",
                table: "LectureFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Video1080p",
                table: "LectureFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video240p",
                table: "LectureFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video360p",
                table: "LectureFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video720p",
                table: "LectureFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
