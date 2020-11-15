using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EShopSolution.Data.Migrations
{
    public partial class SeedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2020, 10, 11, 18, 15, 56, 703, DateTimeKind.Local).AddTicks(906),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 10, 11, 17, 52, 40, 837, DateTimeKind.Local).AddTicks(3533));

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("87d9e782-bc82-4a48-8f05-60b8bb061fb0"), "f13d48d6-ab32-4742-b512-2fcb659759e1", "Administration Role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("909fcfd4-7408-4cb2-a01b-a2e26a484f6b"), new Guid("87d9e782-bc82-4a48-8f05-60b8bb061fb0") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("909fcfd4-7408-4cb2-a01b-a2e26a484f6b"), 0, "544f0d17-cbe7-417a-8fd5-f3dfd0736b15", new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "huasyhai@gmail.com", true, "Hai", "Hua", false, null, "huasyhai@gmail.com", "admin", "AQAAAAEAACcQAAAAEGdarwlV4kdJl4+N2pdaIOfwm5nN2MMHdx+35JAzCoo9vOLANRKD+K8fzo6imVrG0Q==", null, false, "", false, "admin" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2020, 10, 11, 18, 15, 56, 724, DateTimeKind.Local).AddTicks(8815));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("87d9e782-bc82-4a48-8f05-60b8bb061fb0"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("909fcfd4-7408-4cb2-a01b-a2e26a484f6b"), new Guid("87d9e782-bc82-4a48-8f05-60b8bb061fb0") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("909fcfd4-7408-4cb2-a01b-a2e26a484f6b"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 10, 11, 17, 52, 40, 837, DateTimeKind.Local).AddTicks(3533),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 10, 11, 18, 15, 56, 703, DateTimeKind.Local).AddTicks(906));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2020, 10, 11, 17, 52, 40, 854, DateTimeKind.Local).AddTicks(4586));
        }
    }
}
