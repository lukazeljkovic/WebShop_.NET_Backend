using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NajlONline.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adrese",
                columns: table => new
                {
                    AdresaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Broj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adrese", x => x.AdresaID);
                });

            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    KategorijaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivKategorije = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Godine = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.KategorijaID);
                });

            migrationBuilder.CreateTable(
                name: "Sezone",
                columns: table => new
                {
                    SezonaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivSezone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Godina = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sezone", x => x.SezonaID);
                });

            migrationBuilder.CreateTable(
                name: "Uloge",
                columns: table => new
                {
                    KarticaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uloge", x => x.KarticaID);
                });

            migrationBuilder.CreateTable(
                name: "VrsteProizvoda",
                columns: table => new
                {
                    VrstaProizvodaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NazivVrsteProizvoda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NazivPodvrsteProizvoda = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VrsteProizvoda", x => x.VrstaProizvodaID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    KorisnikID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KorisnickoIme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uloga = table.Column<bool>(type: "bit", nullable: false),
                    AdresaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.KorisnikID);
                    table.ForeignKey(
                        name: "FK_Korisnici_Adrese_AdresaID",
                        column: x => x.AdresaID,
                        principalTable: "Adrese",
                        principalColumn: "AdresaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kartice",
                columns: table => new
                {
                    KarticaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KorisnikID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrojRacuna = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumIsteka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVC = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kartice", x => x.KarticaID);
                    table.ForeignKey(
                        name: "FK_Kartice_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proizvodi",
                columns: table => new
                {
                    ProizvodID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VrstaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KategorijaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KorisnikID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SezonaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrenutnoStanje = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodi", x => x.ProizvodID);
                    table.ForeignKey(
                        name: "FK_Proizvodi_Kategorije_KategorijaID",
                        column: x => x.KategorijaID,
                        principalTable: "Kategorije",
                        principalColumn: "KategorijaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proizvodi_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proizvodi_Sezone_SezonaID",
                        column: x => x.SezonaID,
                        principalTable: "Sezone",
                        principalColumn: "SezonaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proizvodi_VrsteProizvoda_VrstaID",
                        column: x => x.VrstaID,
                        principalTable: "VrsteProizvoda",
                        principalColumn: "VrstaProizvodaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kupovine",
                columns: table => new
                {
                    KupovinaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KorisnikID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProizvodID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatumKupovine = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProizvodPorucen = table.Column<bool>(type: "bit", nullable: false),
                    ProizvodDostavljen = table.Column<bool>(type: "bit", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kupovine", x => x.KupovinaID);
                    table.ForeignKey(
                        name: "FK_Kupovine_Korisnici_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Kupovine_Proizvodi_ProizvodID",
                        column: x => x.ProizvodID,
                        principalTable: "Proizvodi",
                        principalColumn: "ProizvodID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kartice_KorisnikID",
                table: "Kartice",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_AdresaID",
                table: "Korisnici",
                column: "AdresaID");

            migrationBuilder.CreateIndex(
                name: "IX_Kupovine_KorisnikID",
                table: "Kupovine",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Kupovine_ProizvodID",
                table: "Kupovine",
                column: "ProizvodID");

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodi_KategorijaID",
                table: "Proizvodi",
                column: "KategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodi_KorisnikID",
                table: "Proizvodi",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodi_SezonaID",
                table: "Proizvodi",
                column: "SezonaID");

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodi_VrstaID",
                table: "Proizvodi",
                column: "VrstaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kartice");

            migrationBuilder.DropTable(
                name: "Kupovine");

            migrationBuilder.DropTable(
                name: "Uloge");

            migrationBuilder.DropTable(
                name: "Proizvodi");

            migrationBuilder.DropTable(
                name: "Kategorije");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Sezone");

            migrationBuilder.DropTable(
                name: "VrsteProizvoda");

            migrationBuilder.DropTable(
                name: "Adrese");
        }
    }
}
