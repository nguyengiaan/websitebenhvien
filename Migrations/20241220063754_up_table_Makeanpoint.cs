using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_table_Makeanpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Makeanappointment",
               columns: table => new
               {
                   Id_Make = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Name_doctor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Examinationtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                   name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Id_Specialty = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Makeanappointment", x => x.Id_Make);
                   table.ForeignKey(
                       name: "FK_Makeanappointment_Specialty_Id_Specialty",
                       column: x => x.Id_Specialty,
                       principalTable: "Specialty",
                       principalColumn: "Id_Specialty",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateIndex(
                name: "IX_Makeanappointment_Id_Specialty",
                table: "Makeanappointment",
                column: "Id_Specialty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
