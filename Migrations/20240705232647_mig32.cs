using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswerRu",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerTr",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionRu",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionTr",
                table: "Faqs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerRu",
                table: "Faqs");

            migrationBuilder.DropColumn(
                name: "AnswerTr",
                table: "Faqs");

            migrationBuilder.DropColumn(
                name: "QuestionRu",
                table: "Faqs");

            migrationBuilder.DropColumn(
                name: "QuestionTr",
                table: "Faqs");
        }
    }
}
