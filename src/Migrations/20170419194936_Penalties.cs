using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mmmsl.Migrations
{
    public partial class Penalties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    GameId = table.Column<int>(nullable: false),
                    MisconductCode = table.Column<string>(nullable: true),
                    PlayerId = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalties_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Penalties_Profiles_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_GameId",
                table: "Penalties",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_MisconductCode",
                table: "Penalties",
                column: "MisconductCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_PlayerId",
                table: "Penalties",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Games");
        }
    }
}
