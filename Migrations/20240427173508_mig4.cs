using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstAdvicedCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecondAdvicedCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advice_Courses_FirstAdvicedCourseId",
                        column: x => x.FirstAdvicedCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advice_Courses_SecondAdvicedCourseId",
                        column: x => x.SecondAdvicedCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advice_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advice_FirstAdvicedCourseId",
                table: "Advice",
                column: "FirstAdvicedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Advice_SecondAdvicedCourseId",
                table: "Advice",
                column: "SecondAdvicedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Advice_TemplateId",
                table: "Advice",
                column: "TemplateId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advice");
        }
    }
}
