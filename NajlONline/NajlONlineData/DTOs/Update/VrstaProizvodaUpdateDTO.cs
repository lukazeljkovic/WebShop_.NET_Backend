using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Update
{
    public class VrstaProizvodaUpdateDTO
    {
        public Guid VrstaProizvodaID { get; set; } 
        public string NazivVrsteProizvoda { get; set; }

        public string NazivPodvrsteProizvoda { get; set; }
    }
}
