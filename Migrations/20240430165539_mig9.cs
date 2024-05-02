using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Subscribers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
