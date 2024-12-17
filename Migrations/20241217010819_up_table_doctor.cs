using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_table_doctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Id_doctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Thumnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Award = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Research = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Training = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Experiencework = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialize = table.Column<int>(type: "int", nullable: false),
                    Id_specialty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id_doctor);
                    table.ForeignKey(
                        name: "FK_Doctor_Specialty_Id_specialty",
                        column: x => x.Id_specialty,
                        principalTable: "Specialty",
                        principalColumn: "Id_Specialty",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feeldoctor",
                columns: table => new
                {
                    Id_Feeldoctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Evaluate = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Doctor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeldoctor", x => x.Id_Feeldoctor);
                    table.ForeignKey(
                        name: "FK_Feeldoctor_Doctor_Id_Doctor",
                        column: x => x.Id_Doctor,
                        principalTable: "Doctor",
                        principalColumn: "Id_doctor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_Id_specialty",
                table: "Doctor",
                column: "Id_specialty");

            migrationBuilder.CreateIndex(
                name: "IX_Feeldoctor_Id_Doctor",
                table: "Feeldoctor",
                column: "Id_Doctor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feeldoctor");

            migrationBuilder.DropTable(
                name: "Doctor");
        }
    }
}
