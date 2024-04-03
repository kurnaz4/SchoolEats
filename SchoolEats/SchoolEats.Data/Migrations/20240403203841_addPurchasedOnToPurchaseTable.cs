using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolEats.Data.Migrations
{
    public partial class addPurchasedOnToPurchaseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PurchasedOn",
                table: "Purchases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasedOn",
                table: "Purchases");
        }
    }
}
