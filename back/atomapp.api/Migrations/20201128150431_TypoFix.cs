using Microsoft.EntityFrameworkCore.Migrations;

namespace atomapp.api.Migrations
{
    public partial class TypoFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishCommend",
                table: "Tsks");

            migrationBuilder.AddColumn<string>(
                name: "FinishComment",
                table: "Tsks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishComment",
                table: "Tsks");

            migrationBuilder.AddColumn<string>(
                name: "FinishCommend",
                table: "Tsks",
                type: "text",
                nullable: true);
        }
    }
}
