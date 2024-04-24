using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro.Respository.Migrations
{
    public partial class add_Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "micro_Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "micro_Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_micro_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_micro_Order_micro_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "micro_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_micro_Product_OrderId",
                table: "micro_Product",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_micro_Order_UserId",
                table: "micro_Order",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_micro_Product_micro_Order_OrderId",
                table: "micro_Product",
                column: "OrderId",
                principalTable: "micro_Order",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_micro_Product_micro_Order_OrderId",
                table: "micro_Product");

            migrationBuilder.DropTable(
                name: "micro_Order");

            migrationBuilder.DropIndex(
                name: "IX_micro_Product_OrderId",
                table: "micro_Product");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "micro_Product");
        }
    }
}
