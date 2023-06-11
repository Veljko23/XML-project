using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class SluzbenikAkcija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SluzbenikAkcije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AutorskoPravoId = table.Column<int>(type: "int", nullable: false),
                    Sifra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumObradeZahteva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Odobren = table.Column<bool>(type: "bit", nullable: false),
                    Referenca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SluzbenikIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SluzbenikPrezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SluzbenikAkcije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SluzbenikAkcije_AutorskaPrava_AutorskoPravoId",
                        column: x => x.AutorskoPravoId,
                        principalTable: "AutorskaPrava",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SluzbenikAkcije_AutorskoPravoId",
                table: "SluzbenikAkcije",
                column: "AutorskoPravoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SluzbenikAkcije");
        }
    }
}
