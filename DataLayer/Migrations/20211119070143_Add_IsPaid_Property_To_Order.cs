using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class Add_IsPaid_Property_To_Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "530e8661-f58b-41d2-b644-c606d487c979");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffc5d616-fd64-45b4-9e87-a9dd47bacd1f");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9f9ec9b8-2e40-427d-937f-b67945c4c4c4", "ae0d1e3e-6e50-4a2b-95b7-3ecb20270ae7", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6e4b8c59-6c7e-47e8-aa79-e873ce61f016", "93f190e8-95df-432e-8187-57661b83641b", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e4b8c59-6c7e-47e8-aa79-e873ce61f016");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f9ec9b8-2e40-427d-937f-b67945c4c4c4");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ffc5d616-fd64-45b4-9e87-a9dd47bacd1f", "87e16727-960f-424b-a24a-f909d7aa42f4", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "530e8661-f58b-41d2-b644-c606d487c979", "ef468b3b-5ead-445a-84f5-e8edf2f80e50", "Administrator", "ADMINISTRATOR" });
        }
    }
}
