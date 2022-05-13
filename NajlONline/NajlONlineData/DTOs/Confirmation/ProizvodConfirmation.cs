using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Confirmation
{
    public class ProizvodConfirmation
    {
        public Guid ProizvodID { get; set; }
        public string Naziv { get; set; }
        public string Cena { get; set; }
    }
}
