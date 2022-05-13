using NajlONline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs
{
    public class KupovinaDTO
    {
        public Guid KupovinaID { get; set; } 
        public KorisnikModel Korisnik { get; set; }
        public ProizvodModel Proizvod { get; set; }
        public DateTime DatumKupovine { get; set; }
        public bool ProizvodPorucen { get; set; }
        public bool ProizvodDostavljen { get; set; }
        public int Kolicina { get; set; }
    }
}
