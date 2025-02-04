using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_register : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registerhealth",
                columns: table => new
                {
                    Id_Registerhealth = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Examinationtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registerhealth", x => x.Id_Registerhealth);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registerhealth");
        }
    }
}
