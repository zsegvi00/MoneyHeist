using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.Migrations
{
    public partial class Heist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Heist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeistSkill",
                columns: table => new
                {
                    HeistId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<string>(type: "TEXT", nullable: false),
                    Members = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeistSkill", x => new { x.HeistId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_HeistSkill_Heist_HeistId",
                        column: x => x.HeistId,
                        principalTable: "Heist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeistSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Heist_Name",
                table: "Heist",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeistSkill_SkillId",
                table: "HeistSkill",
                column: "SkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeistSkill");

            migrationBuilder.DropTable(
                name: "Heist");
        }
    }
}
