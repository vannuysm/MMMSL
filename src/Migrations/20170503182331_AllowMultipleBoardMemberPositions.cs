using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mmmsl.Migrations
{
    public partial class AllowMultipleBoardMemberPositions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardMembers",
                table: "BoardMembers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BoardMembers",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardMembers",
                table: "BoardMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BoardMembers_ProfileId",
                table: "BoardMembers",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardMembers",
                table: "BoardMembers");

            migrationBuilder.DropIndex(
                name: "IX_BoardMembers_ProfileId",
                table: "BoardMembers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BoardMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardMembers",
                table: "BoardMembers",
                column: "ProfileId");
        }
    }
}
