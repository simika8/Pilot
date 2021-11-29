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
                name: "demoproductexts",
                columns: table => new
                {
                    productid = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    minimumstock = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_demoproductexts", x => x.productid);
                });

            migrationBuilder.CreateTable(
                name: "demoproducts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: true),
                    releasedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    extproductid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_demoproducts", x => x.id);
                    table.ForeignKey(
                        name: "fk_demoproducts_demoproductext_exttempid",
                        column: x => x.extproductid,
                        principalTable: "demoproductexts",
                        principalColumn: "productid");
                });

            migrationBuilder.CreateTable(
                name: "demoinventorystocks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    productid = table.Column<Guid>(type: "uuid", nullable: false),
                    storeid = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    demoproductid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_demoinventorystocks", x => x.id);
                    table.ForeignKey(
                        name: "fk_demoinventorystocks_demoproducts_demoproductid",
                        column: x => x.demoproductid,
                        principalTable: "demoproducts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_demoinventorystocks_demoproductid",
                table: "demoinventorystocks",
                column: "demoproductid");

            migrationBuilder.CreateIndex(
                name: "ix_demoproducts_extproductid",
                table: "demoproducts",
                column: "extproductid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "demoinventorystocks");

            migrationBuilder.DropTable(
                name: "demoproducts");

            migrationBuilder.DropTable(
                name: "demoproductexts");
        }
    }
}
