using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace who_am_i_be.Migrations
{
    /// <inheritdoc />
    public partial class Players : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Characters_CharacterId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Lobbies_LobbyId",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.RenameIndex(
                name: "IX_Player_LobbyId",
                table: "Players",
                newName: "IX_Players_LobbyId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_CharacterId",
                table: "Players",
                newName: "IX_Players_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Lobbies_LobbyId",
                table: "Players",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Characters_CharacterId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Lobbies_LobbyId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.RenameIndex(
                name: "IX_Players_LobbyId",
                table: "Player",
                newName: "IX_Player_LobbyId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_CharacterId",
                table: "Player",
                newName: "IX_Player_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Characters_CharacterId",
                table: "Player",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Lobbies_LobbyId",
                table: "Player",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
