using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro.Respository.Migrations
{
    public partial class add_ProductVersionnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Version",
                table: "micro_Product",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldRowVersion: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Version",
                table: "micro_Product",
                type: "time",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true);
        }
    }
}
