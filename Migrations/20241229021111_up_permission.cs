using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionUser_Permissions_id_Permission",
                table: "PermissionUser");



            migrationBuilder.CreateIndex(
                name: "IX_PermissionUser_id_PermissionUser",
                table: "PermissionUser",
                column: "id_PermissionUser");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionUser_Permissions_id_PermissionUser",
                table: "PermissionUser",
                column: "id_PermissionUser",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionUser_Permissions_id_PermissionUser",
                table: "PermissionUser");

            migrationBuilder.DropIndex(
                name: "IX_PermissionUser_id_PermissionUser",
                table: "PermissionUser");

            migrationBuilder.AlterColumn<int>(
                name: "id_Permission",
                table: "PermissionUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionUser_Permissions_id_Permission",
                table: "PermissionUser",
                column: "id_Permission",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
