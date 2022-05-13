using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.DTOs.Confirmation
{
    public class KorisnikConfirmation
    {
        public Guid KorisnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
    }
}
