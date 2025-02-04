using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class addrecruitimentpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recruitmentpost",
                columns: table => new
                {
                    id_recruitmentpost = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),


                    title_recruitmentpost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_recruitmentpost = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content_recruitmentpost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),






                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruitmentpost", x => x.id_recruitmentpost);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recruitmentpost");
        }
    }
}
