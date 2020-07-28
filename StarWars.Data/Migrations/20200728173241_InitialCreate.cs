using Microsoft.EntityFrameworkCore.Migrations;

namespace StarWars.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EpisodeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "CharacterEpisode",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterEpisode", x => new { x.CharacterId, x.EpisodeId });
                    table.ForeignKey(
                        name: "FK_CharacterEpisode_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterEpisode_Episodes_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Characters",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Luke Skywalker" },
                    { 2, "Darth Vader" },
                    { 3, "Han Solo" },
                    { 4, "Leia Organa" },
                    { 5, "C-3PO" },
                    { 6, "R2-D2" },
                    { 7, "Wilhuff Tarkin" }
                });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "EpisodeName" },
                values: new object[,]
                {
                    { 1, "NEWHOPE" },
                    { 2, "EMPIRE" },
                    { 3, "JEDI" }
                });

            migrationBuilder.InsertData(
                table: "CharacterEpisode",
                columns: new[] { "CharacterId", "EpisodeId" },
                values: new object[,]
                {
                    { 6, 3 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 7, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 1, 2 },
                    { 1, 1 },
                    { 2, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 },
                    { 3, 2 },
                    { 5, 3 }
                });

            migrationBuilder.InsertData(
                table: "CharacterFriend",
                columns: new[] { "CharacterId", "CharacterFriendId" },
                values: new object[,]
                {
                    { 7, 2 },
                    { 6, 3 },
                    { 1, 4 },
                    { 1, 6 },
                    { 1, 5 },
                    { 2, 7 },
                    { 3, 1 },
                    { 3, 4 },
                    { 3, 6 },
                    { 6, 4 },
                    { 4, 1 },
                    { 4, 5 },
                    { 4, 6 },
                    { 5, 1 },
                    { 5, 3 },
                    { 5, 4 },
                    { 5, 6 },
                    { 6, 1 },
                    { 4, 3 },
                    { 1, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterEpisode_EpisodeId",
                table: "CharacterEpisode",
                column: "EpisodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterEpisode");

            migrationBuilder.DropTable(
                name: "CharacterFriend");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
