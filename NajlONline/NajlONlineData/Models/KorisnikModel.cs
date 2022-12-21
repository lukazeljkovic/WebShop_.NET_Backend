using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class KorisnikModel
    {
        [Key]
        public Guid KorisnikID { get; set; } = Guid.NewGuid();
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public Guid UlogaID { get; set; }
        public UlogaModel Uloga { get; set; }
        public Guid AdresaID { get; set; }
        public AdresaModel Adresa { get; set; }
        
        
    }
}
