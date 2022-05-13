using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineServices.Parameters
{
    public class KorisnikParameters : QueryStringParameters
    {
        public KorisnikParameters()
        {
            OrderBy = "Ime";
        }

        public string KorisnickoIme { get; set; }
    }
}
