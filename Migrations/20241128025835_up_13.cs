using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id_menu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title_menu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link_menu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Order_menu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id_menu);
                });

            migrationBuilder.CreateTable(
                name: "MenuChild",
                columns: table => new
                {
                    Id_MenuChild = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title_MenuChild = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link_MenuChild = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_menu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Order_menu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuChild", x => x.Id_MenuChild);
                    table.ForeignKey(
                        name: "FK_MenuChild_Menu_Id_menu",
                        column: x => x.Id_menu,
                        principalTable: "Menu",
                        principalColumn: "Id_menu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuChild_Id_menu",
                table: "MenuChild",
                column: "Id_menu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuChild");

            migrationBuilder.DropTable(
                name: "Menu");
        }
    }
}
