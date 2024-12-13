using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class Chat__table_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_AspNetUsers_ApplicationUserId",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_ApplicationUserId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Chat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Chat",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_ApplicationUserId",
                table: "Chat",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_AspNetUsers_ApplicationUserId",
                table: "Chat",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
