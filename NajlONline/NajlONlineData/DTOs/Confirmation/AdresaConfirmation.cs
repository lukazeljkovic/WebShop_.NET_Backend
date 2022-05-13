using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Confirmation
{
    public class AdresaConfirmation
    {
        public Guid AdresaID { get; set; } 
        public string Ulica { get; set; }
        public string Broj { get; set; }
    }
}
