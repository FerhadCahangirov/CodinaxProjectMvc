using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodinaxProjectMvc.Migrations
{
    public partial class mig32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceInfo_Price_PriceId",
                table: "PriceInfo");

            migrationBuilder.AlterColumn<Guid>(
                name: "PriceId",
                table: "PriceInfo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceInfo_Price_PriceId",
                table: "PriceInfo",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceInfo_Price_PriceId",
                table: "PriceInfo");

            migrationBuilder.AlterColumn<Guid>(
                name: "PriceId",
                table: "PriceInfo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceInfo_Price_PriceId",
                table: "PriceInfo",
                column: "PriceId",
                principalTable: "Price",
                principalColumn: "Id");
        }
    }
}
