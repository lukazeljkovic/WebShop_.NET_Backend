using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Models
{
    public class SezonaModel
    {
        [Key]
        public Guid SezonaID { get; set; } = Guid.NewGuid();
        public string NazivSezone { get; set; }
        public string Godina { get; set; }
    }
}
