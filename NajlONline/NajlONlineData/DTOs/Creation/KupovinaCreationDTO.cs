using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Creation
{
   public class KupovinaCreationDTO
    {
        public Guid KorisnikID { get; set; }
        public Guid ProizvodID { get; set; }
        public DateTime DatumKupovine { get; set; }
        public bool ProizvodPorucen { get; set; }
        public bool ProizvodDostavljen { get; set; }
        public int Kolicina { get; set; }
    }
}
