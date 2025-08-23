using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_add_table_menu_nav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Titlemenu",
                columns: table => new
                {
                    Id_titlemenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url_icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titlemenu", x => x.Id_titlemenu);
                });

            migrationBuilder.CreateTable(
                name: "Menunav",
                columns: table => new
                {
                    Id_menunav = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Id_titlemenu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menunav", x => x.Id_menunav);
                    table.ForeignKey(
                        name: "FK_Menunav_Titlemenu_Id_titlemenu",
                        column: x => x.Id_titlemenu,
                        principalTable: "Titlemenu",
                        principalColumn: "Id_titlemenu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Menunav_Id_titlemenu",
                table: "Menunav",
                column: "Id_titlemenu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menunav");

            migrationBuilder.DropTable(
                name: "Titlemenu");
        }
    }
}
