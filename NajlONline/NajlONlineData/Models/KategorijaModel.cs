using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class KategorijaModel
    {
        [Key]
        public Guid KategorijaID { get; set; } = Guid.NewGuid();
        public string NazivKategorije { get; set; }
        public string Godine { get; set; }
        
    }
}
