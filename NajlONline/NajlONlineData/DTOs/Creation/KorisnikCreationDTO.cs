using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.DTOs.Creation
{
    public class KorisnikCreationDTO
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public Guid UlogaID { get; set; }  
        public Guid AdresaID { get; set; }
        
    }
}
