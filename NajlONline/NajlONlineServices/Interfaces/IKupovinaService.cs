using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services.Interfaces
{
    public interface IKupovinaService
    {
        List<KupovinaModel> GetAll(KupovinaParameters kupovinaParameters);
        KupovinaModel GetByID(Guid id);
        KupovinaConfirmation Add(KupovinaModel kupovinaModel);
        KupovinaConfirmation Update(KupovinaModel kupovinaModel);
        void Delete(Guid kupovinaID);
        List<KupovinaModel> GetByKorisnikID(Guid id);
        void ApplySort(ref IQueryable<KupovinaModel> kupovine, string orderByQueryString);

    }
}
