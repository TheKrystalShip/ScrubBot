using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ScrubBot.Database.Migrations
{
    public partial class Maintenance3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AuditChannelId = table.Column<string>(nullable: true),
                    CharPrefix = table.Column<string>(nullable: true),
                    IconUrl = table.Column<string>(nullable: true),
                    MemberCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    StringPrefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 20, nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(maxLength: 20, nullable: true),
                    GuildId = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(maxLength: 50, nullable: true),
                    TimezoneOffset = table.Column<int>(nullable: true),
                    Username = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_GuildId",
                table: "Users",
                column: "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
