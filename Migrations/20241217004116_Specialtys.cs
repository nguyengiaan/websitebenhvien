using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class Specialtys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Specialty",
                columns: table => new
                {
                    Id_Specialty = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Machine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Achievement = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialty", x => x.Id_Specialty);
                });

            migrationBuilder.CreateTable(
                name: "ListvideoSpectialty",
                columns: table => new
                {
                    Id_ListvideoSpectialty = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Specialty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListvideoSpectialty", x => x.Id_ListvideoSpectialty);
                    table.ForeignKey(
                        name: "FK_ListvideoSpectialty_Specialty_Id_Specialty",
                        column: x => x.Id_Specialty,
                        principalTable: "Specialty",
                        principalColumn: "Id_Specialty",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Postrelate",
                columns: table => new
                {
                    Id_Postrelate = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alias_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Createat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_Specialty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postrelate", x => x.Id_Postrelate);
                    table.ForeignKey(
                        name: "FK_Postrelate_Specialty_Id_Specialty",
                        column: x => x.Id_Specialty,
                        principalTable: "Specialty",
                        principalColumn: "Id_Specialty",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListvideoSpectialty_Id_Specialty",
                table: "ListvideoSpectialty",
                column: "Id_Specialty");

            migrationBuilder.CreateIndex(
                name: "IX_Postrelate_Id_Specialty",
                table: "Postrelate",
                column: "Id_Specialty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListvideoSpectialty");

            migrationBuilder.DropTable(
                name: "Postrelate");

            migrationBuilder.DropTable(
                name: "Specialty");
        }
    }
}
