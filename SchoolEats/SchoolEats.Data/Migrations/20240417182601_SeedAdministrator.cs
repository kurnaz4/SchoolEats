using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolEats.Data.Migrations
{
    public partial class SeedAdministrator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsApproved", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("809ec051-8e26-4200-9248-9c4815ff3af2"), 0, "4eff5749-8f73-470d-b2b7-fb3c6fd7e6f0", "administrator@gmail.com", false, true, true, null, "ADMINISTRATOR@GMAIL.COM", "АДМИНАДМИНОВ", "AQAAAAEAACcQAAAAEElb00msN19oCULHAmkvl/UaLyHRRVFXXg8PH7/fWIkzXaH2/vaFqyfpIxqiXcRcXg==", null, false, "6SMIWUAQOBJRLP7BOERO2MQNLDTVGMPA", false, "АдминАдминов" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("809ec051-8e26-4200-9248-9c4815ff3af2"));
        }
    }
}
