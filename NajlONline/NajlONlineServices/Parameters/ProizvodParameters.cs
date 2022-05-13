using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineServices.Parameters
{
   public class ProizvodParameters : QueryStringParameters
    {
        public ProizvodParameters()
        {
            OrderBy = "Naziv";
        }

        public string Naziv { get; set; }
        public string Vrsta { get; set; }
        public string PodVrsta { get; set; }
        public string Kategorija { get; set; }
        public string Godine { get; set; }
        public string Sezona { get; set; }
        public string KorisnickoIme { get; set; }
        public string Velicina { get; set; }

        
    }
}
