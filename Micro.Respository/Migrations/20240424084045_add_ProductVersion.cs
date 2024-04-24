using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro.Respository.Migrations
{
    public partial class add_ProductVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_micro_Product_micro_Order_OrderId",
                table: "micro_Product");

            migrationBuilder.DropIndex(
                name: "IX_micro_Product_OrderId",
                table: "micro_Product");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "micro_Product");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Version",
                table: "micro_Product",
                type: "time",
                rowVersion: true,
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "micro_Product");

            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "micro_Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_micro_Product_OrderId",
                table: "micro_Product",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_micro_Product_micro_Order_OrderId",
                table: "micro_Product",
                column: "OrderId",
                principalTable: "micro_Order",
                principalColumn: "Id");
        }
    }
}
