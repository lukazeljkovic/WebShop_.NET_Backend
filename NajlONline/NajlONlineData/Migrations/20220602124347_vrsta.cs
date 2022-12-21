using Microsoft.EntityFrameworkCore.Migrations;

namespace NajlONline.Migrations
{
    public partial class vrsta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proizvodi_VrsteProizvoda_VrstaID",
                table: "Proizvodi");

            migrationBuilder.RenameColumn(
                name: "VrstaID",
                table: "Proizvodi",
                newName: "VrstaProizvodaID");

            migrationBuilder.RenameIndex(
                name: "IX_Proizvodi_VrstaID",
                table: "Proizvodi",
                newName: "IX_Proizvodi_VrstaProizvodaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Proizvodi_VrsteProizvoda_VrstaProizvodaID",
                table: "Proizvodi",
                column: "VrstaProizvodaID",
                principalTable: "VrsteProizvoda",
                principalColumn: "VrstaProizvodaID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proizvodi_VrsteProizvoda_VrstaProizvodaID",
                table: "Proizvodi");

            migrationBuilder.RenameColumn(
                name: "VrstaProizvodaID",
                table: "Proizvodi",
                newName: "VrstaID");

            migrationBuilder.RenameIndex(
                name: "IX_Proizvodi_VrstaProizvodaID",
                table: "Proizvodi",
                newName: "IX_Proizvodi_VrstaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Proizvodi_VrsteProizvoda_VrstaID",
                table: "Proizvodi",
                column: "VrstaID",
                principalTable: "VrsteProizvoda",
                principalColumn: "VrstaProizvodaID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
