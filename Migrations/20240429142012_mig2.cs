using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Features");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
