using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_forbusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forbusiness",
                columns: table => new
                {
                    Id_Forbusiness = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Forbusiness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content_Forbusiness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status_Forbusiness = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forbusiness", x => x.Id_Forbusiness);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forbusiness");
        }
    }
}
