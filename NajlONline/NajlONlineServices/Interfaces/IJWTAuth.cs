using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineServices.Interfaces
{
    public interface IJWTAuth
    {
        string Authentication(string username, string password);
    }
}
