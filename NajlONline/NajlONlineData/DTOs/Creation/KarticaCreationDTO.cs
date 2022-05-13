using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Creation
{
    public class KarticaCreationDTO
    {
        public Guid KorisnikID { get; set; }
        public string BrojRacuna { get; set; }
        public string DatumIsteka { get; set; }
        public string CVC { get; set; }
    }
}
