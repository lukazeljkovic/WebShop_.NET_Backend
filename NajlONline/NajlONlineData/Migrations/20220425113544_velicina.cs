using Microsoft.EntityFrameworkCore.Migrations;

namespace NajlONline.Migrations
{
    public partial class velicina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Velicina",
                table: "Proizvodi",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Velicina",
                table: "Proizvodi");
        }
    }
}
