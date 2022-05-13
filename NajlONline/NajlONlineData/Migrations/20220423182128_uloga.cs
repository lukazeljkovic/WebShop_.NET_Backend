using Microsoft.EntityFrameworkCore.Migrations;

namespace NajlONline.Migrations
{
    public partial class uloga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uloga",
                table: "Korisnici");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Uloga",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
