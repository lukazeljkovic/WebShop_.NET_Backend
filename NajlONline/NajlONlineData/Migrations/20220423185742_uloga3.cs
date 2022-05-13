using Microsoft.EntityFrameworkCore.Migrations;

namespace NajlONline.Migrations
{
    public partial class uloga3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KarticaID",
                table: "Uloge",
                newName: "UlogaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UlogaID",
                table: "Uloge",
                newName: "KarticaID");
        }
    }
}
