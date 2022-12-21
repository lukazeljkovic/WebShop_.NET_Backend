using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class ProizvodModel
    {
        [Key]
        public Guid ProizvodID { get; set; } = Guid.NewGuid();
        public Guid VrstaProizvodaID { get; set; }
        public VrstaProizvodaModel VrstaProizvoda { get; set; }
        public Guid KategorijaID { get; set; }
        public KategorijaModel Kategorija { get; set; }
        public Guid KorisnikID { get; set; }
        public KorisnikModel Korisnik { get; set; }
        public Guid SezonaID { get; set; }
        public SezonaModel Sezona { get; set; }
        public string Naziv { get; set; }
        public string Cena { get; set; }
        public string Velicina { get; set; }
        public int TrenutnoStanje { get; set; }
    }
}
