using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class addmenuser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuAdmin_AspNetUsers_iduser",
                table: "MenuAdmin");

            migrationBuilder.DropIndex(
                name: "IX_MenuAdmin_iduser",
                table: "MenuAdmin");

            migrationBuilder.DropColumn(
                name: "iduser",
                table: "MenuAdmin");

            migrationBuilder.CreateTable(
                name: "MenuAdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MenuAdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuAdminUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuAdminUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuAdminUsers_MenuAdmin_MenuAdminId",
                        column: x => x.MenuAdminId,
                        principalTable: "MenuAdmin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuAdminUsers_MenuAdminId",
                table: "MenuAdminUsers",
                column: "MenuAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuAdminUsers_UserId",
                table: "MenuAdminUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuAdminUsers");

            migrationBuilder.AddColumn<string>(
                name: "iduser",
                table: "MenuAdmin",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuAdmin_iduser",
                table: "MenuAdmin",
                column: "iduser");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuAdmin_AspNetUsers_iduser",
                table: "MenuAdmin",
                column: "iduser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
