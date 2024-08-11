using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaffeBar.Migrations
{
    /// <inheritdoc />
    public partial class RemadeReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Reservations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
