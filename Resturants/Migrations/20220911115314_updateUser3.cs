using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturants.Migrations
{
    public partial class updateUser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_vendors_VendorId",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_menu_vendors_VendorId",
                table: "menu");

            migrationBuilder.DropForeignKey(
                name: "FK_photos_vendors_VendorId",
                table: "photos");

            migrationBuilder.DropTable(
                name: "vendors");

            migrationBuilder.DropIndex(
                name: "IX_photos_VendorId",
                table: "photos");

            migrationBuilder.DropIndex(
                name: "IX_menu_VendorId",
                table: "menu");

            migrationBuilder.DropIndex(
                name: "IX_addresses_VendorId",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "menu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "menu",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCompelte = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkDays = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkHours = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_photos_VendorId",
                table: "photos",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_menu_VendorId",
                table: "menu",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_VendorId",
                table: "addresses",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_vendors_VendorId",
                table: "addresses",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_menu_vendors_VendorId",
                table: "menu",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_photos_vendors_VendorId",
                table: "photos",
                column: "VendorId",
                principalTable: "vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
