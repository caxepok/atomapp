using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace atomapp.api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TskComments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TskId = table.Column<long>(nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(nullable: false),
                    CreatorId = table.Column<long>(nullable: false),
                    CreatorName = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    AudioGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TskComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TskTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Class = table.Column<int>(nullable: false),
                    Period = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TskTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<long>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    WorkplaceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tsks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<long>(nullable: false),
                    ExecutorId = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    PlannedAt = table.Column<DateTimeOffset>(nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsFinished = table.Column<bool>(nullable: false),
                    ExecutionPercent = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    TaskObject = table.Column<string>(nullable: true),
                    AudioGuid = table.Column<string>(nullable: true),
                    FinishCommend = table.Column<string>(nullable: true),
                    FinishAudioGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tsks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tsks_Workers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tsks_Workers_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tsks_CreatorId",
                table: "Tsks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tsks_ExecutorId",
                table: "Tsks",
                column: "ExecutorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TskComments");

            migrationBuilder.DropTable(
                name: "Tsks");

            migrationBuilder.DropTable(
                name: "TskTemplates");

            migrationBuilder.DropTable(
                name: "Workplaces");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
