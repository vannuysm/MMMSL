using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mmmsl.Migrations
{
    public partial class BetterPenalties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Penalties",
                newName: "TeamId");

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "Penalties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_TeamId",
                table: "Penalties",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_Teams_TeamId",
                table: "Penalties",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_Teams_TeamId",
                table: "Penalties");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_TeamId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Penalties");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Penalties",
                newName: "Type");
        }
    }
}
