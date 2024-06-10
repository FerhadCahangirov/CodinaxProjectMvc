using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "Progresses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Progresses");
        }
    }
}
