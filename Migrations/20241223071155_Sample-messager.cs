using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class Samplemessager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SampleMessage",
                columns: table => new
                {
                    Id_SampleMessager = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleMessage", x => x.Id_SampleMessager);
                });

            migrationBuilder.CreateTable(
                name: "ButtonSample",
                columns: table => new
                {
                    Id_ButtonSample = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_SampleMessage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ButtonSample", x => x.Id_ButtonSample);
                    table.ForeignKey(
                        name: "FK_ButtonSample_SampleMessage_Id_SampleMessage",
                        column: x => x.Id_SampleMessage,
                        principalTable: "SampleMessage",
                        principalColumn: "Id_SampleMessager",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ButtonSample_Id_SampleMessage",
                table: "ButtonSample",
                column: "Id_SampleMessage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ButtonSample");

            migrationBuilder.DropTable(
                name: "SampleMessage");
        }
    }
}
