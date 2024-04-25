using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                newName: "ProfileImagePathOrContainer");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfileImagePathOrContainer",
                table: "AspNetUsers",
                newName: "ProfileImageUrl");
        }
    }
}
