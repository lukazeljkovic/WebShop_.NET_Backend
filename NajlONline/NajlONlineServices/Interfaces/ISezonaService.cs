using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services.Interfaces
{
    public interface ISezonaService
    {
        List<SezonaModel> GetAll(SezonaParameters sezonaParameters);
        SezonaModel GetByID(Guid id);
        SezonaConfirmation Add(SezonaModel sezonaModel);
        SezonaConfirmation Update(SezonaModel sezonaModel);
        void Delete(Guid sezonaID);
        void ApplySort(ref IQueryable<SezonaModel> sezone, string orderByQueryString);
    }
}
