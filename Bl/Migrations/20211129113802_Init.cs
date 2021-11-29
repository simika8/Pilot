using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bl.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemoProductExt",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MinimumStock = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoProductExt", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "DemoProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ExtProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoProducts_DemoProductExt_ExtProductId",
                        column: x => x.ExtProductId,
                        principalTable: "DemoProductExt",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "DemoInventoryStock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    DemoProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoInventoryStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoInventoryStock_DemoProducts_DemoProductId",
                        column: x => x.DemoProductId,
                        principalTable: "DemoProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoInventoryStock_DemoProductId",
                table: "DemoInventoryStock",
                column: "DemoProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoProducts_ExtProductId",
                table: "DemoProducts",
                column: "ExtProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoInventoryStock");

            migrationBuilder.DropTable(
                name: "DemoProducts");

            migrationBuilder.DropTable(
                name: "DemoProductExt");
        }
    }
}
