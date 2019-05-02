using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrubBot.Database.SQLite.Migrations
{
    public partial class addSubscribeMessageIdtoEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscribeMessageId",
                table: "Events",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscribeMessageId",
                table: "Events");
        }
    }
}
