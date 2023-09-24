using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace RineaR.Spring.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SchedulerJobState",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    LastRunTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulerJobState", x => x.Id);
                })
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
                    MarvelousScore = table.Column<int>(type: "int", nullable: false),
                    PainfulScore = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActionType = table.Column<string>(type: "longtext", nullable: false),
                    WakeUpId = table.Column<int>(type: "int", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DiscordMessageId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    DiscordReactionId = table.Column<int>(type: "int", nullable: true),
                    TargetUserId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    TargetPainfulScore = table.Column<int>(type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true),
                    Login_ApplicationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Praise_DiscordMessageId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    Praise_DiscordReactionId = table.Column<int>(type: "int", nullable: true),
                    Praise_TargetUserId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    TargetMarvelousScore = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_ActionBase_User_Praise_TargetUserId",
                        column: x => x.Praise_TargetUserId,
                        principalTable: "User",
                        principalColumn: "DiscordID",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_ActionBase_Praise_TargetUserId",
                table: "ActionBase",
                column: "Praise_TargetUserId");

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
                name: "SchedulerJobState");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
