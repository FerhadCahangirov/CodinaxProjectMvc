using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advices");

            migrationBuilder.AddColumn<Guid>(
                name: "FirstAdvicedCourseId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SecondAdvicedCourseId",
                table: "Courses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FirstAdvicedCourseId",
                table: "Courses",
                column: "FirstAdvicedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SecondAdvicedCourseId",
                table: "Courses",
                column: "SecondAdvicedCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_FirstAdvicedCourseId",
                table: "Courses",
                column: "FirstAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_SecondAdvicedCourseId",
                table: "Courses",
                column: "SecondAdvicedCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_FirstAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_SecondAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_FirstAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SecondAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FirstAdvicedCourseId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SecondAdvicedCourseId",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "Advices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstAdvicedCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecondAdvicedCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advices_Courses_FirstAdvicedCourseId",
                        column: x => x.FirstAdvicedCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advices_Courses_SecondAdvicedCourseId",
                        column: x => x.SecondAdvicedCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advices_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advices_FirstAdvicedCourseId",
                table: "Advices",
                column: "FirstAdvicedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Advices_SecondAdvicedCourseId",
                table: "Advices",
                column: "SecondAdvicedCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Advices_TemplateId",
                table: "Advices",
                column: "TemplateId",
                unique: true);
        }
    }
}
