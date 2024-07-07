using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentTr",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "Tools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentTr",
                table: "Tools",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FutureJobDescRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FutureJobDescTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FutureJobSalaryRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FutureJobSalaryTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadingDescriptionRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadingDescriptionTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadingRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadingTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertiesRu",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertiesTr",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "PriceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentTr",
                table: "PriceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleRu",
                table: "Price",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleTr",
                table: "Price",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "FutureJobTitles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentTr",
                table: "FutureJobTitles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleRu",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleTr",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentRu",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentTr",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "ContentTr",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "Tools");

            migrationBuilder.DropColumn(
                name: "ContentTr",
                table: "Tools");

            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "ContentTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "DescriptionTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "FutureJobDescRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "FutureJobDescTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "FutureJobSalaryRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "FutureJobSalaryTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "HeadingDescriptionRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "HeadingDescriptionTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "HeadingRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "HeadingTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "LocationRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "LocationTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "PropertiesRu",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "PropertiesTr",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "PriceInfo");

            migrationBuilder.DropColumn(
                name: "ContentTr",
                table: "PriceInfo");

            migrationBuilder.DropColumn(
                name: "TitleRu",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "TitleTr",
                table: "Price");

            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "FutureJobTitles");

            migrationBuilder.DropColumn(
                name: "ContentTr",
                table: "FutureJobTitles");

            migrationBuilder.DropColumn(
                name: "TitleRu",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TitleTr",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ContentRu",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ContentTr",
                table: "Categories");
        }
    }
}
