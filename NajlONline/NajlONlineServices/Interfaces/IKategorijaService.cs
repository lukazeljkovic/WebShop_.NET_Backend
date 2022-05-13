using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services.Interfaces
{
    public interface IKategorijaService
    {
        List<KategorijaModel> GetAll(KategorijaParameters kategorijaParameters);
        KategorijaModel GetByID(Guid id);
        KategorijaConfirmation Add(KategorijaModel kategorijaModel);
        KategorijaConfirmation Update(KategorijaModel kategorijaModel);
        void Delete(Guid kategorijaID);
        void ApplySort(ref IQueryable<KategorijaModel> kategorija, string orderByQueryString);


    }
}
