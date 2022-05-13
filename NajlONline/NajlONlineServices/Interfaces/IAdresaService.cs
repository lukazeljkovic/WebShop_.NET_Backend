
using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services
{
    public interface IAdresaService
    {
        List<AdresaModel> GetAll(AdresaParameters adresaParameters);
        AdresaModel GetByID(Guid id);
        AdresaConfirmation Add(AdresaModel adresaModel);
        AdresaConfirmation Update(AdresaModel adresaModel);
        void Delete(Guid adresaID);
        void ApplySort(ref IQueryable<AdresaModel> adrese, string orderByQueryString);
    }
}
