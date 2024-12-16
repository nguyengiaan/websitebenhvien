using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_news_table_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
             name: "PK_News",
             table: "News");

            migrationBuilder.DropColumn(
                name: "Id_News",
                table: "News");

            // Add the new Id_News column with IDENTITY property
            migrationBuilder.AddColumn<int>(
                name: "Id_News",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Add the primary key constraint back
            migrationBuilder.AddPrimaryKey(
                name: "PK_News",
                table: "News",
                column: "Id_News");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id_News",
                table: "News",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
