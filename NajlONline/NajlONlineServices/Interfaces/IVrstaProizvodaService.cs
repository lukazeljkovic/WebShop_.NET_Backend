using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services.Interfaces
{
    public interface IVrstaProizvodaService
    {
        List<VrstaProizvodaModel> GetAll(VrstaProizvodaParameters vrstaProizvodaParameters);
        VrstaProizvodaModel GetByID(Guid id);
        VrstaProizvodaConfirmation Add(VrstaProizvodaModel vrstaProizvodaModel);
        VrstaProizvodaConfirmation Update(VrstaProizvodaModel vrstaProizvodaModel);
        void Delete(Guid vrstaProizvodaID);
        void ApplySort(ref IQueryable<VrstaProizvodaModel> vrsteProizvoda, string orderByQueryString);
    }
}
