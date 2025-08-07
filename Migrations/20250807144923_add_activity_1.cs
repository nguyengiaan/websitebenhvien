using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class add_activity_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Postactivity",
                columns: table => new
                {
                    Id_Postactivity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Alias_url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Descriptionshort = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Id_Categoryactivity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    SchemaMakup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postactivity", x => x.Id_Postactivity);
                    table.ForeignKey(
                        name: "FK_Postactivity_Activitycategory_Id_Categoryactivity",
                        column: x => x.Id_Categoryactivity,
                        principalTable: "Activitycategory",
                        principalColumn: "Id_activitycategory",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Postactivity_Id_Categoryactivity",
                table: "Postactivity",
                column: "Id_Categoryactivity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Postactivity");
        }
    }
}
