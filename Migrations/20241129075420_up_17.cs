using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoryproduct",
                columns: table => new
                {
                    Id_Categoryproduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoryproduct", x => x.Id_Categoryproduct);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id_product = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    Id_Categoryproduct = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id_product);
                    table.ForeignKey(
                        name: "FK_Product_Categoryproduct_Id_Categoryproduct",
                        column: x => x.Id_Categoryproduct,
                        principalTable: "Categoryproduct",
                        principalColumn: "Id_Categoryproduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proimages",
                columns: table => new
                {
                    Id_proimages = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_product = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proimages", x => x.Id_proimages);
                    table.ForeignKey(
                        name: "FK_Proimages_Product_Id_product",
                        column: x => x.Id_product,
                        principalTable: "Product",
                        principalColumn: "Id_product",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Id_Categoryproduct",
                table: "Product",
                column: "Id_Categoryproduct");

            migrationBuilder.CreateIndex(
                name: "IX_Proimages_Id_product",
                table: "Proimages",
                column: "Id_product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proimages");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Categoryproduct");
        }
    }
}
