using Microsoft.EntityFrameworkCore.Migrations;

namespace StarWars.Data.Migrations
{
    public partial class fixassocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Characters_CharacterId",
                table: "Episodes");

            migrationBuilder.DropIndex(
                name: "IX_Episodes_CharacterId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Episodes");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterEpisodes_Characters_CharacterId",
                table: "CharacterEpisodes",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterEpisodes_Characters_CharacterId",
                table: "CharacterEpisodes");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Episodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_CharacterId",
                table: "Episodes",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Characters_CharacterId",
                table: "Episodes",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
