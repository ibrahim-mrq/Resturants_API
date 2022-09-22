using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturants.Migrations
{
    public partial class addTootlePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProdcutCount",
                table: "Carts",
                newName: "TotleProduct");

            migrationBuilder.AddColumn<float>(
                name: "TotlePrice",
                table: "Carts",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotlePrice",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "TotleProduct",
                table: "Carts",
                newName: "ProdcutCount");
        }
    }
}
