using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class KupovinaModel
    {
        [Key]
        public Guid KupovinaID { get; set; } = Guid.NewGuid();
        public Guid KorisnikID { get; set; }
        public KorisnikModel Korisnik { get; set; }
        public Guid ProizvodID { get; set; }
        public ProizvodModel Proizvod { get; set; }
        public DateTime DatumKupovine { get; set; }
        public bool ProizvodPorucen { get; set; }
        public bool ProizvodDostavljen { get; set; }
        public bool UspesnaKupovina { get; set; }
        public int Kolicina { get; set; }

    }
}
