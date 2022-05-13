using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NajlONline.Migrations
{
    public partial class uloga2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UlogaID",
                table: "Korisnici",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_UlogaID",
                table: "Korisnici",
                column: "UlogaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnici_Uloge_UlogaID",
                table: "Korisnici",
                column: "UlogaID",
                principalTable: "Uloge",
                principalColumn: "KarticaID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Korisnici_Uloge_UlogaID",
                table: "Korisnici");

            migrationBuilder.DropIndex(
                name: "IX_Korisnici_UlogaID",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "UlogaID",
                table: "Korisnici");
        }
    }
}
