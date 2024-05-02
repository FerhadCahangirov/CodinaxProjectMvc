using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TokenValidationKey",
                table: "Subscribers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenValidationKey",
                table: "Subscribers");
        }
    }
}
