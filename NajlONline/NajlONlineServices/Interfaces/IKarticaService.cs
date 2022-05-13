using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services
{
   public interface IKarticaService
    {
        List<KarticaModel> GetAll(KarticaParameters karticaParameters);
        KarticaModel GetByID(Guid id);
        KarticaConfirmation Add(KarticaModel karticaModel);
        KarticaConfirmation Update(KarticaModel karticaModel);
        void Delete(Guid karticaID);
        void ApplySort(ref IQueryable<KarticaModel> kartice, string orderByQueryString);
    }
}
