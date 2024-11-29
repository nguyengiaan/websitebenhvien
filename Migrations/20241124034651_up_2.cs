using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Header",
                columns: table => new
                {
                    Id_header = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Header", x => x.Id_header);
                });

            migrationBuilder.CreateTable(
                name: "Titleheader",
                columns: table => new
                {
                    Id_titleheader = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_header = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titleheader", x => x.Id_titleheader);
                    table.ForeignKey(
                        name: "FK_Titleheader_Header_Id_header",
                        column: x => x.Id_header,
                        principalTable: "Header",
                        principalColumn: "Id_header",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Titleheader_Id_header",
                table: "Titleheader",
                column: "Id_header");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Titleheader");

            migrationBuilder.DropTable(
                name: "Header");
        }
    }
}
