using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class Addmenuadminuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
