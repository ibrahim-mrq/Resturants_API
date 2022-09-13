using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturants.Migrations
{
    public partial class updateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token",
                table: "users",
                newName: "Token");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "users",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkDays",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkHours",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "photos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "menu",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "addresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_photos_UserId",
                table: "photos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_menu_UserId",
                table: "menu",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_UserId",
                table: "addresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_users_UserId",
                table: "addresses",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_menu_users_UserId",
                table: "menu",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_photos_users_UserId",
                table: "photos",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_users_UserId",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_users_UserId",
                table: "menu");

            migrationBuilder.DropForeignKey(
                name: "FK_photos_users_UserId",
                table: "photos");

            migrationBuilder.DropIndex(
                name: "IX_photos_UserId",
                table: "photos");

            migrationBuilder.DropIndex(
                name: "IX_menu_UserId",
                table: "menu");

            migrationBuilder.DropIndex(
                name: "IX_addresses_UserId",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "users");

            migrationBuilder.DropColumn(
                name: "WorkDays",
                table: "users");

            migrationBuilder.DropColumn(
                name: "WorkHours",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "photos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "menu");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "addresses");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "users",
                newName: "token");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }
    }
}
