using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class WorkSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workschedule",
                columns: table => new
                {
                    Id_workschedule = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Morning = table.Column<bool>(type: "bit", nullable: false),
                    Afternoon = table.Column<bool>(type: "bit", nullable: false),
                    Evening = table.Column<bool>(type: "bit", nullable: false),
                    Id_doctor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workschedule", x => x.Id_workschedule);
                    table.ForeignKey(
                        name: "FK_Workschedule_Doctor_Id_doctor",
                        column: x => x.Id_doctor,
                        principalTable: "Doctor",
                        principalColumn: "Id_doctor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workschedule_Id_doctor",
                table: "Workschedule",
                column: "Id_doctor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workschedule");
        }
    }
}
