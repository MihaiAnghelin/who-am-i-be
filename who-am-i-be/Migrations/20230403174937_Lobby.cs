using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace who_am_i_be.Migrations
{
    /// <inheritdoc />
    public partial class Lobby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "Players");

            migrationBuilder.AddColumn<bool>(
                name: "HasGameStarted",
                table: "Lobbies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CategoryLobby",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LobbiesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryLobby", x => new { x.CategoriesId, x.LobbiesId });
                    table.ForeignKey(
                        name: "FK_CategoryLobby_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryLobby_Lobbies_LobbiesId",
                        column: x => x.LobbiesId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLobby_LobbiesId",
                table: "CategoryLobby",
                column: "LobbiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryLobby");

            migrationBuilder.DropColumn(
                name: "HasGameStarted",
                table: "Lobbies");

            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "Players",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
