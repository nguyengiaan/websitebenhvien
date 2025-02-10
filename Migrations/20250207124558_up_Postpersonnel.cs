using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_Postpersonnel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Postpersonnel",
                columns: table => new
                {
                    id_recruitmentpost = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content_recruitmentpost = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    Date_recruitmentpost = table.Column<DateTime>(type: "datetime2", maxLength: 2147483647, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    Statuson = table.Column<bool>(type: "bit", maxLength: 2147483647, nullable: false),
                    title_recruitmentpost = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postpersonnel", x => x.id_recruitmentpost);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Postpersonnel");

            migrationBuilder.CreateTable(
                name: "Postrecruitment",
                columns: table => new
                {
                    id_recruitmentpost = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postrecruitment", x => x.id_recruitmentpost);
                });
        }
    }
}
