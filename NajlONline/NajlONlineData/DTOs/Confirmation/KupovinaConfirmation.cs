using NajlONline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Confirmation
{
   public class KupovinaConfirmation
    {
        public Guid KupovinaID { get; set; }
        public DateTime DatumKupovine { get; set; }
        public bool ProizvodPorucen { get; set; }
    }
}
