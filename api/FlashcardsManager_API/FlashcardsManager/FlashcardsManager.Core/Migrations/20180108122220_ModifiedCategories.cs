using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FlashcardsManager.Core.Migrations
{
    public partial class ModifiedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Availability",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_CreatorId",
                table: "Categories",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_CreatorId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CreatorId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Categories");
        }
    }
}
