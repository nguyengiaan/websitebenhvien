using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Footer",
                columns: table => new
                {
                    Id_footer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkgt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linktn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkdnbs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linktd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkkb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkxn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkcdha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linknt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkdn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkface = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linktwiter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkyoutube = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Footer", x => x.Id_footer);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Footer");
        }
    }
}
