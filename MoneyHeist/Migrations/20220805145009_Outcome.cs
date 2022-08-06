using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.Migrations
{
    public partial class Outcome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Outcome",
                table: "Heists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "Heists");
        }
    }
}
