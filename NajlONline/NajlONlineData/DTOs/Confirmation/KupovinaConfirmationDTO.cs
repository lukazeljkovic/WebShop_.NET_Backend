using NajlONline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Confirmation
{
   public class KupovinaConfirmationDTO
    {
        public DateTime DatumKupovine { get; set; }
        public Guid KupovinaID { get; set; }
    }
}
