using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class UlogaModel
    {
        [Key]
        public Guid UlogaID { get; set; } = Guid.NewGuid();
        public String Naziv { get; set; }

    }
}
