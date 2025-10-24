using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class guidecustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catogeryguider",
                columns: table => new
                {
                    Id_Catogeryguider = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catogeryguider", x => x.Id_Catogeryguider);
                });

            migrationBuilder.CreateTable(
                name: "Customerguide",
                columns: table => new
                {
                    Id_Customerguide = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descriptionshort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id_Categorynews = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    SchemaMakup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customerguide", x => x.Id_Customerguide);
                    table.ForeignKey(
                        name: "FK_Customerguide_Catogeryguider_Id_Customerguide",
                        column: x => x.Id_Customerguide,
                        principalTable: "Catogeryguider",
                        principalColumn: "Id_Catogeryguider",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customerguide");

            migrationBuilder.DropTable(
                name: "Catogeryguider");
        }
    }
}
