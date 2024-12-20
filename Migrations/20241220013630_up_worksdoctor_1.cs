using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class up_worksdoctor_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workschedule_Doctor_Id_doctor",
                table: "Workschedule");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Workschedule",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Id_doctor",
                table: "Workschedule",
                newName: "Id_worksdoctor");

            migrationBuilder.RenameIndex(
                name: "IX_Workschedule_Id_doctor",
                table: "Workschedule",
                newName: "IX_Workschedule_Id_worksdoctor");

            migrationBuilder.AlterColumn<string>(
                name: "Morning",
                table: "Workschedule",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Evening",
                table: "Workschedule",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Afternoon",
                table: "Workschedule",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "Worksdoctor",
                columns: table => new
                {
                    Id_worksdoctor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_doctor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worksdoctor", x => x.Id_worksdoctor);
                    table.ForeignKey(
                        name: "FK_Worksdoctor_Doctor_Id_doctor",
                        column: x => x.Id_doctor,
                        principalTable: "Doctor",
                        principalColumn: "Id_doctor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Worksdoctor_Id_doctor",
                table: "Worksdoctor",
                column: "Id_doctor");

            migrationBuilder.AddForeignKey(
                name: "FK_Workschedule_Worksdoctor_Id_worksdoctor",
                table: "Workschedule",
                column: "Id_worksdoctor",
                principalTable: "Worksdoctor",
                principalColumn: "Id_worksdoctor",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workschedule_Worksdoctor_Id_worksdoctor",
                table: "Workschedule");

            migrationBuilder.DropTable(
                name: "Worksdoctor");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Workschedule",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Id_worksdoctor",
                table: "Workschedule",
                newName: "Id_doctor");

            migrationBuilder.RenameIndex(
                name: "IX_Workschedule_Id_worksdoctor",
                table: "Workschedule",
                newName: "IX_Workschedule_Id_doctor");

            migrationBuilder.AlterColumn<bool>(
                name: "Morning",
                table: "Workschedule",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Evening",
                table: "Workschedule",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Afternoon",
                table: "Workschedule",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Workschedule_Doctor_Id_doctor",
                table: "Workschedule",
                column: "Id_doctor",
                principalTable: "Doctor",
                principalColumn: "Id_doctor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
