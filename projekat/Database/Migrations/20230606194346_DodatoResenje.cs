using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class DodatoResenje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SluzbenikAkcije_AutorskaPrava_AutorskoPravoId",
                table: "SluzbenikAkcije");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SluzbenikAkcije",
                table: "SluzbenikAkcije");

            migrationBuilder.RenameTable(
                name: "SluzbenikAkcije",
                newName: "Resenja");

            migrationBuilder.RenameIndex(
                name: "IX_SluzbenikAkcije_AutorskoPravoId",
                table: "Resenja",
                newName: "IX_Resenja_AutorskoPravoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resenja",
                table: "Resenja",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resenja_AutorskaPrava_AutorskoPravoId",
                table: "Resenja",
                column: "AutorskoPravoId",
                principalTable: "AutorskaPrava",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resenja_AutorskaPrava_AutorskoPravoId",
                table: "Resenja");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resenja",
                table: "Resenja");

            migrationBuilder.RenameTable(
                name: "Resenja",
                newName: "SluzbenikAkcije");

            migrationBuilder.RenameIndex(
                name: "IX_Resenja_AutorskoPravoId",
                table: "SluzbenikAkcije",
                newName: "IX_SluzbenikAkcije_AutorskoPravoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SluzbenikAkcije",
                table: "SluzbenikAkcije",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SluzbenikAkcije_AutorskaPrava_AutorskoPravoId",
                table: "SluzbenikAkcije",
                column: "AutorskoPravoId",
                principalTable: "AutorskaPrava",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
