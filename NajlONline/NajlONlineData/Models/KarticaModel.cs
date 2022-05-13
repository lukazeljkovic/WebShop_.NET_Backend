using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class KarticaModel
    {
        [Key]
        public Guid KarticaID { get; set; } = Guid.NewGuid();
        public Guid KorisnikID { get; set; }
        public KorisnikModel Korisnik { get; set; }
        public string BrojRacuna { get; set; }
        public string DatumIsteka { get; set; }
        public string CVC { get; set; }

    }
}
