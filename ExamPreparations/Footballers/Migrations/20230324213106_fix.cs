using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Footballers.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Footballers_Teams_TeamId",
                table: "Footballers");

            migrationBuilder.DropIndex(
                name: "IX_Footballers_TeamId",
                table: "Footballers");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Footballers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Footballers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Footballers_TeamId",
                table: "Footballers",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Footballers_Teams_TeamId",
                table: "Footballers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
