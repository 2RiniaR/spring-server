using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace RineaR.Spring.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    DiscordID = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GitHubID = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.DiscordID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ActionBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ActionType = table.Column<string>(type: "longtext", nullable: false),
                    WakeUpId = table.Column<int>(type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true),
                    DiscordMessageId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    DiscordReactionId = table.Column<int>(type: "int", nullable: true),
                    TargetUserId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    GivenScore = table.Column<int>(type: "int", nullable: true),
                    BedInId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionBase_ActionBase_BedInId",
                        column: x => x.BedInId,
                        principalTable: "ActionBase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionBase_User_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "User",
                        principalColumn: "DiscordID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionBase_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "DiscordID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ActionBase_BedInId",
                table: "ActionBase",
                column: "BedInId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionBase_TargetUserId",
                table: "ActionBase",
                column: "TargetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionBase_UserId",
                table: "ActionBase",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionBase");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
