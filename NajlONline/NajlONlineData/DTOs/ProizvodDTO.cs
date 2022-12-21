using NajlONline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs
{
    public class ProizvodDTO
    {
        public Guid ProizvodID { get; set; } 
       
        public VrstaProizvodaModel VrstaProizvoda { get; set; }
        
        public KategorijaModel Kategorija { get; set; }
        
        public KorisnikModel Korisnik { get; set; }
        
        public SezonaModel Sezona { get; set; }
        public string Naziv { get; set; }
        public string Cena { get; set; }
        public string Velicina { get; set; }
        public int TrenutnoStanje { get; set; }
    }
}
