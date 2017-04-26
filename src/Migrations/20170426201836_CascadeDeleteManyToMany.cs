using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mmmsl.Migrations
{
    public partial class CascadeDeleteManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RosterPlayers_Profiles_ProfileId",
                table: "RosterPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_RosterPlayers_Teams_TeamId",
                table: "RosterPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamManagers_Profiles_ProfileId",
                table: "TeamManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamManagers_Teams_TeamId",
                table: "TeamManagers");

            migrationBuilder.AddForeignKey(
                name: "FK_RosterPlayers_Profiles_ProfileId",
                table: "RosterPlayers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RosterPlayers_Teams_TeamId",
                table: "RosterPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamManagers_Profiles_ProfileId",
                table: "TeamManagers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamManagers_Teams_TeamId",
                table: "TeamManagers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RosterPlayers_Profiles_ProfileId",
                table: "RosterPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_RosterPlayers_Teams_TeamId",
                table: "RosterPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamManagers_Profiles_ProfileId",
                table: "TeamManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamManagers_Teams_TeamId",
                table: "TeamManagers");

            migrationBuilder.AddForeignKey(
                name: "FK_RosterPlayers_Profiles_ProfileId",
                table: "RosterPlayers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RosterPlayers_Teams_TeamId",
                table: "RosterPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamManagers_Profiles_ProfileId",
                table: "TeamManagers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamManagers_Teams_TeamId",
                table: "TeamManagers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
