using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class VrstaProizvodaModel
    {
        [Key]
        public Guid VrstaProizvodaID { get; set; } = Guid.NewGuid();
        public string NazivVrsteProizvoda { get; set; }

        public string NazivPodvrsteProizvoda { get; set; }

    }
}
