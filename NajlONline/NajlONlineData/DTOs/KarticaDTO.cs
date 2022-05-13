using NajlONline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs
{
   public class KarticaDTO
    {
        public Guid KarticaID { get; set; } 
        
        public KorisnikModel Korisnik { get; set; }
        public string BrojRacuna { get; set; }
        public string DatumIsteka { get; set; }
        public string CVC { get; set; }
    }
}
