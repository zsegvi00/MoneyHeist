using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.Migrations
{
    public partial class addmigrationHeists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeistSkill_Heist_HeistId",
                table: "HeistSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_HeistSkill_Skills_SkillId",
                table: "HeistSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeistSkill",
                table: "HeistSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Heist",
                table: "Heist");

            migrationBuilder.RenameTable(
                name: "HeistSkill",
                newName: "HeistSkills");

            migrationBuilder.RenameTable(
                name: "Heist",
                newName: "Heists");

            migrationBuilder.RenameIndex(
                name: "IX_HeistSkill_SkillId",
                table: "HeistSkills",
                newName: "IX_HeistSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_Heist_Name",
                table: "Heists",
                newName: "IX_Heists_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeistSkills",
                table: "HeistSkills",
                columns: new[] { "HeistId", "SkillId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Heists",
                table: "Heists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeistSkills_Heists_HeistId",
                table: "HeistSkills",
                column: "HeistId",
                principalTable: "Heists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeistSkills_Skills_SkillId",
                table: "HeistSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeistSkills_Heists_HeistId",
                table: "HeistSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_HeistSkills_Skills_SkillId",
                table: "HeistSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeistSkills",
                table: "HeistSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Heists",
                table: "Heists");

            migrationBuilder.RenameTable(
                name: "HeistSkills",
                newName: "HeistSkill");

            migrationBuilder.RenameTable(
                name: "Heists",
                newName: "Heist");

            migrationBuilder.RenameIndex(
                name: "IX_HeistSkills_SkillId",
                table: "HeistSkill",
                newName: "IX_HeistSkill_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_Heists_Name",
                table: "Heist",
                newName: "IX_Heist_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeistSkill",
                table: "HeistSkill",
                columns: new[] { "HeistId", "SkillId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Heist",
                table: "Heist",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeistSkill_Heist_HeistId",
                table: "HeistSkill",
                column: "HeistId",
                principalTable: "Heist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeistSkill_Skills_SkillId",
                table: "HeistSkill",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
