using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineServices.Parameters
{
    public class SezonaParameters : QueryStringParameters
    {
        public SezonaParameters()
        {
            OrderBy = "NazivSezone";
        }
    }
}
