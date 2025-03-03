using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class table_Specialty_remove_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Achievement",
                table: "Specialty");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Specialty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Achievement",
                table: "Specialty",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "Specialty",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
