using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyHeist.Migrations
{
    public partial class HeistSkillsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeistSkills_Skills_SkillId",
                table: "HeistSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeistSkills",
                table: "HeistSkills");

            migrationBuilder.DropIndex(
                name: "IX_HeistSkills_SkillId",
                table: "HeistSkills");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "HeistSkills",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HeistSkills",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "HeistSkills",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeistSkills",
                table: "HeistSkills",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_HeistSkills_HeistId",
                table: "HeistSkills",
                column: "HeistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HeistSkills",
                table: "HeistSkills");

            migrationBuilder.DropIndex(
                name: "IX_HeistSkills_HeistId",
                table: "HeistSkills");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "HeistSkills");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "HeistSkills",
                newName: "SkillId");

            migrationBuilder.AlterColumn<int>(
                name: "SkillId",
                table: "HeistSkills",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeistSkills",
                table: "HeistSkills",
                columns: new[] { "HeistId", "SkillId" });

            migrationBuilder.CreateIndex(
                name: "IX_HeistSkills_SkillId",
                table: "HeistSkills",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeistSkills_Skills_SkillId",
                table: "HeistSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
