using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskManager.Migrations
{
    /// <inheritdoc />
    public partial class RelationUserDesk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desks_Users_UserId",
                table: "Desks");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Desks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Desks_Users_UserId",
                table: "Desks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desks_Users_UserId",
                table: "Desks");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Desks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Desks_Users_UserId",
                table: "Desks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
