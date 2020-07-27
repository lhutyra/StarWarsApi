using Microsoft.EntityFrameworkCore.Migrations;

namespace StarWars.Data.Migrations
{
    public partial class friendassociationi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterEpisodes_Characters_CharacterId",
                table: "CharacterEpisodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Characters_CharacterId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_CharacterId",
                table: "Characters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterEpisodes",
                table: "CharacterEpisodes");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Characters");

            migrationBuilder.RenameTable(
                name: "CharacterEpisodes",
                newName: "CharacterEpisode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterEpisode",
                table: "CharacterEpisode",
                columns: new[] { "CharacterId", "EpisodeId" });

            migrationBuilder.CreateTable(
                name: "CharacterFriend",
                columns: table => new
                {
                    CharacterId = table.Column<int>(nullable: false),
                    CharacterFriendId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFriend", x => new { x.CharacterId, x.CharacterFriendId });
                    table.ForeignKey(
                        name: "FK_CharacterFriend_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterEpisode_Characters_CharacterId",
                table: "CharacterEpisode",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterEpisode_Characters_CharacterId",
                table: "CharacterEpisode");

            migrationBuilder.DropTable(
                name: "CharacterFriend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterEpisode",
                table: "CharacterEpisode");

            migrationBuilder.RenameTable(
                name: "CharacterEpisode",
                newName: "CharacterEpisodes");

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterEpisodes",
                table: "CharacterEpisodes",
                columns: new[] { "CharacterId", "EpisodeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CharacterId",
                table: "Characters",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterEpisodes_Characters_CharacterId",
                table: "CharacterEpisodes",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Characters_CharacterId",
                table: "Characters",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
