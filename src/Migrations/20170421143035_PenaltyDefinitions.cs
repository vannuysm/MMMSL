using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using mmmsl.Models;

namespace mmmsl.Migrations
{
    public partial class PenaltyDefinitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Penalties_MisconductCode",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Penalties");

            migrationBuilder.CreateTable(
                name: "PenaltyDefinitions",
                columns: table => new
                {
                    MisconductCode = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    Severity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PenaltyDefinitions", x => x.MisconductCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_MisconductCode",
                table: "Penalties",
                column: "MisconductCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_PenaltyDefinitions_MisconductCode",
                table: "Penalties",
                column: "MisconductCode",
                principalTable: "PenaltyDefinitions",
                principalColumn: "MisconductCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_PenaltyDefinitions_MisconductCode",
                table: "Penalties");

            migrationBuilder.DropTable(
                name: "PenaltyDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_MisconductCode",
                table: "Penalties");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Penalties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Penalties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "Penalties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_MisconductCode",
                table: "Penalties",
                column: "MisconductCode",
                unique: true);
        }
    }
}
