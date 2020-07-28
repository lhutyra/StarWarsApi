using Microsoft.EntityFrameworkCore.Migrations;

namespace StarWars.Data.Migrations
{
    public partial class newjunctiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterWithFriend",
                columns: table => new
                {
                    CharacterId = table.Column<int>(nullable: false),
                    CharacterFriendId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterWithFriend", x => new { x.CharacterId, x.CharacterFriendId });
                    table.ForeignKey(
                        name: "FK_CharacterWithFriend_Characters_CharacterFriendId",
                        column: x => x.CharacterFriendId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterWithFriend_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterWithFriend_CharacterFriendId",
                table: "CharacterWithFriend",
                column: "CharacterFriendId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterWithFriend");
        }
    }
}
