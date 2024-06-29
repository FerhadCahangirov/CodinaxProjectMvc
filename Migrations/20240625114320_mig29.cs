using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Video144p",
                table: "LectureFiles");

            migrationBuilder.DropColumn(
                name: "Video480p",
                table: "LectureFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Video144p",
                table: "LectureFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video480p",
                table: "LectureFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
