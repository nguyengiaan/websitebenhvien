using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class addcreateat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create_at",
                table: "Forbusiness",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create_at",
                table: "Forbusiness");
        }
    }
}
