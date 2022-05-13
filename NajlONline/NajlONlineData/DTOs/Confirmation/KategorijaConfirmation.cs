using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Confirmation
{
    public class KategorijaConfirmation
    {
        public Guid KategorijaID { get; set; } = Guid.NewGuid();
        public string NazivKategorije { get; set; }
        
    }
}
