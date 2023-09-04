using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskManager.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationUserDesk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Desks_DeskId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DeskId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeskId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Desks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Desks_UserId",
                table: "Desks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Desks_Users_UserId",
                table: "Desks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desks_Users_UserId",
                table: "Desks");

            migrationBuilder.DropIndex(
                name: "IX_Desks_UserId",
                table: "Desks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Desks");

            migrationBuilder.AddColumn<int>(
                name: "DeskId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeskId",
                table: "Users",
                column: "DeskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Desks_DeskId",
                table: "Users",
                column: "DeskId",
                principalTable: "Desks",
                principalColumn: "Id");
        }
    }
}
