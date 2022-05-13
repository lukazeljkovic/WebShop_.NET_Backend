using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services
{
   public interface IUlogaService
    {
        List<UlogaModel> GetAll(UlogaParameters ulogaParameters);
        UlogaModel GetByID(Guid id);
        UlogaConfirmation Add(UlogaModel ulogaModel);
        UlogaConfirmation Update(UlogaModel ulogaModel);
        void Delete(Guid ulogaID);
        void ApplySort(ref IQueryable<UlogaModel> uloge, string orderByQueryString);



    }
}
