using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaffeBar.Migrations
{
    /// <inheritdoc />
    public partial class AddedImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_IdentityUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "MenuItems",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Events",
                newName: "ImagePath");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_IdentityUserId",
                table: "Orders",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_IdentityUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "MenuItems",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Events",
                newName: "Image");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Reservations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Orders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_IdentityUserId",
                table: "Orders",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_IdentityUserId",
                table: "Reservations",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
