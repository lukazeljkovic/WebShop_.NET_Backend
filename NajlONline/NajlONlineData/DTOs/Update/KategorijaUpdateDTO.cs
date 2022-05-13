using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Update
{
    public class KategorijaUpdateDTO
    {
        public Guid KategorijaID { get; set; } 
        public string NazivKategorije { get; set; }
        public string Godine { get; set; }
    }
}
