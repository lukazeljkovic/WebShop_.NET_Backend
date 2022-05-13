using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Update
{
    public class SezonaUpdateDTO
    {
        public Guid SezonaID { get; set; } 
        public string NazivSezone { get; set; }
        public string Godina { get; set; }
    }
}
