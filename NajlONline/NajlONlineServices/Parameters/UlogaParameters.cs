using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineServices.Parameters
{
    public class UlogaParameters : QueryStringParameters
    {
        public UlogaParameters()
        {
            OrderBy = "Naziv";
        }
    }
}
