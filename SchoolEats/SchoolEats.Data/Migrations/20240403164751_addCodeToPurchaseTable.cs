using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolEats.Data.Migrations
{
    public partial class addCodeToPurchaseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Purchases");
        }
    }
}
