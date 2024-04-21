using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Micro.Respository.Migrations
{
    public partial class addlv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Lv",
                table: "micro_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lv",
                table: "micro_Users");
        }
    }
}
