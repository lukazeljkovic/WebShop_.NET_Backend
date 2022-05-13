using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class AdresaModel
    {
        [Key]
        public Guid AdresaID { get; set; } = Guid.NewGuid();
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Grad { get; set; }


    }
}
