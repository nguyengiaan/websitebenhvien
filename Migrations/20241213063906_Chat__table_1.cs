using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class Chat__table_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_AspNetUsers_Id_Sender",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_Id_Sender",
                table: "Chat");

            migrationBuilder.AlterColumn<string>(
                name: "Id_Sender",
                table: "Chat",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Id_Sender",
                table: "Chat",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_Id_Sender",
                table: "Chat",
                column: "Id_Sender");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_AspNetUsers_Id_Sender",
                table: "Chat",
                column: "Id_Sender",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
