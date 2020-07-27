using Microsoft.EntityFrameworkCore.Migrations;

namespace StarWars.Data.Migrations
{
    public partial class referenceupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CharacterEpisode_EpisodeId",
                table: "CharacterEpisode",
                column: "EpisodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterEpisode_Episodes_EpisodeId",
                table: "CharacterEpisode",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterEpisode_Episodes_EpisodeId",
                table: "CharacterEpisode");

            migrationBuilder.DropIndex(
                name: "IX_CharacterEpisode_EpisodeId",
                table: "CharacterEpisode");
        }
    }
}
