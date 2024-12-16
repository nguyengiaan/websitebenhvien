using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_news_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Bỏ khóa chính
            migrationBuilder.DropPrimaryKey(
                name: "PK_News",
                table: "News");

            // Thay đổi kiểu dữ liệu của cột Id_News
            migrationBuilder.AlterColumn<int>(
                name: "Id_News",
                table: "News",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Thêm lại khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id_News");

            // Thay đổi kiểu dữ liệu của cột Alias_url và tạo chỉ mục duy nhất
            migrationBuilder.AlterColumn<string>(
                name: "Alias_url",
                table: "News",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_News_Alias_url",
                table: "News",
                column: "Alias_url",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Bỏ chỉ mục duy nhất
            migrationBuilder.DropIndex(
                name: "IX_News_Alias_url",
                table: "News");

            // Bỏ khóa chính
            migrationBuilder.DropPrimaryKey(
                name: "PK_News",
                table: "News");

            // Thay đổi kiểu dữ liệu của cột Id_News về kiểu cũ
            migrationBuilder.AlterColumn<string>(
                name: "Id_News",
                table: "News",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Thêm lại khóa chính
            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id_News");

            // Thay đổi kiểu dữ liệu của cột Alias_url về kiểu cũ
            migrationBuilder.AlterColumn<string>(
                name: "Alias_url",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

    }
}
