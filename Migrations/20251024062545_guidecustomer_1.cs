using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace websitebenhvien.Migrations
{
    /// <inheritdoc />
    public partial class guidecustomer_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customerguide_Catogeryguider_Id_Customerguide",
                table: "Customerguide");
            

            // Keep existing Id_Categorynews column to preserve data; do not drop it here.
            // migrationBuilder.DropColumn(name: "Id_Categorynews", table: "Customerguide");

            // NOTE: do not alter identity property on existing Id_Customerguide column
            // Add new FK column as nullable initially to avoid altering identity on an existing column
            migrationBuilder.AddColumn<int>(
                name: "Id_Catogeryguider",
                table: "Customerguide",
                type: "int",
                nullable: true);

            // Try to copy values from previous string column (if they contain numeric ids)
            migrationBuilder.Sql(@"
                UPDATE Customerguide
                SET Id_Catogeryguider = TRY_CAST(Id_Categorynews AS INT)
                WHERE Id_Categorynews IS NOT NULL AND TRY_CAST(Id_Categorynews AS INT) IS NOT NULL
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Customerguide_Id_Catogeryguider",
                table: "Customerguide",
                column: "Id_Catogeryguider");

            migrationBuilder.AddForeignKey(
                name: "FK_Customerguide_Catogeryguider_Id_Catogeryguider",
                table: "Customerguide",
                column: "Id_Catogeryguider",
                principalTable: "Catogeryguider",
                principalColumn: "Id_Catogeryguider",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customerguide_Catogeryguider_Id_Catogeryguider",
                table: "Customerguide");

            migrationBuilder.DropIndex(
                name: "IX_Customerguide_Id_Catogeryguider",
                table: "Customerguide");

            migrationBuilder.DropColumn(
                name: "Id_Catogeryguider",
                table: "Customerguide");

            migrationBuilder.AddColumn<string>(
                name: "Id_Categorynews",
                table: "Customerguide",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Customerguide_Catogeryguider_Id_Customerguide",
                table: "Customerguide",
                column: "Id_Customerguide",
                principalTable: "Catogeryguider",
                principalColumn: "Id_Catogeryguider",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
