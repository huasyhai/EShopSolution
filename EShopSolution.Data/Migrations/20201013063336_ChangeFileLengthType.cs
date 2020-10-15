using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EShopSolution.Data.Migrations
{
    public partial class ChangeFileLengthType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("87d9e782-bc82-4a48-8f05-60b8bb061fb0"),
                column: "ConcurrencyStamp",
                value: "0f6050db-c0c0-46be-ad99-a88f9eb6ef7e");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("909fcfd4-7408-4cb2-a01b-a2e26a484f6b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ddc5507a-dbb9-4da5-9d69-47fa78c719e1", "AQAAAAEAACcQAAAAEKj048TJyZgPxtbrojUdYZz5bKkcRnEiz385+t6nkgihEZwcPvLOWIBLXY3sdBOjSA==" });

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
                value: new DateTime(2020, 10, 13, 14, 33, 35, 514, DateTimeKind.Local).AddTicks(859));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("87d9e782-bc82-4a48-8f05-60b8bb061fb0"),
                column: "ConcurrencyStamp",
                value: "ac0ea8f6-4fad-4133-836b-b9be744dc728");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("909fcfd4-7408-4cb2-a01b-a2e26a484f6b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "07d19d78-7540-4a6c-a16e-059f100eccb7", "AQAAAAEAACcQAAAAENjDK6nZ4+OJfSg9O3IOLdKGbXf8iQKzRjEv+x+aQ/oUlVkz7IwKk3GUvZGTh4GVlw==" });

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
                value: new DateTime(2020, 10, 13, 10, 33, 59, 194, DateTimeKind.Local).AddTicks(4620));
        }
    }
}
