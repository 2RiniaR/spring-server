using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RineaR.Spring.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WakeUpId",
                table: "ActionBase");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WakeUpId",
                table: "ActionBase",
                type: "int",
                nullable: true);
        }
    }
}
