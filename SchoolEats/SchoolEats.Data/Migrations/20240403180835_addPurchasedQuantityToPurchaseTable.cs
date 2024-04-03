using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolEats.Data.Migrations
{
    public partial class addPurchasedQuantityToPurchaseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchasedQuantity",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasedQuantity",
                table: "Purchases");
        }
    }
}