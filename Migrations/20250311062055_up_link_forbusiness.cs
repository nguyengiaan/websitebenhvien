using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_link_forbusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon_Forbusiness",
                table: "Forbusiness",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "link_Forbusiness",
                table: "Forbusiness",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "link_Forbusiness_1",
                table: "Forbusiness",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon_Forbusiness",
                table: "Forbusiness");

            migrationBuilder.DropColumn(
                name: "link_Forbusiness",
                table: "Forbusiness");

            migrationBuilder.DropColumn(
                name: "link_Forbusiness_1",
                table: "Forbusiness");
        }
    }
}
