using NajlONline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.DTOs
{
    public class KorisnikDTO
    {
        public Guid KorisnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        
        public UlogaModel Uloga { get; set; }
      
        public AdresaModel Adresa { get; set; }
    }
}
