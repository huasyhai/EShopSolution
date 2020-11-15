using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EShopSolution.Data.Migrations
{
    public partial class addProductImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 10, 11, 18, 15, 56, 703, DateTimeKind.Local).AddTicks(906));

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(maxLength: 200, nullable: false),
                    Caption = table.Column<string>(maxLength: 200, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    FileSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 10, 11, 18, 15, 56, 703, DateTimeKind.Local).AddTicks(906),
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("87d9e782-bc82-4a48-8f05-60b8bb061fb0"),
                column: "ConcurrencyStamp",
                value: "f13d48d6-ab32-4742-b512-2fcb659759e1");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("909fcfd4-7408-4cb2-a01b-a2e26a484f6b"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "544f0d17-cbe7-417a-8fd5-f3dfd0736b15", "AQAAAAEAACcQAAAAEGdarwlV4kdJl4+N2pdaIOfwm5nN2MMHdx+35JAzCoo9vOLANRKD+K8fzo6imVrG0Q==" });

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
    }
}
