﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace FWTL.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelegramSession");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelegramSession",
                columns: table => new
                {
                    HashId = table.Column<string>(nullable: false),
                    Session = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelegramSession", x => x.HashId);
                });
        }
    }
}
