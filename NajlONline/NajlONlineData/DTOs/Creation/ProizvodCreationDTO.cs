using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Creation
{
    public class ProizvodCreationDTO
    {
        public Guid VrstaID { get; set; }
        public Guid KategorijaID { get; set; }
        public Guid KorisnikID { get; set; }
        
        public Guid SezonaID { get; set; }
        public string Naziv { get; set; }
        public string Cena { get; set; }
        public string Velicina { get; set; }
        public int TrenutnoStanje { get; set; }
    }
}
