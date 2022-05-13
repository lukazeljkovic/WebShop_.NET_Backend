using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services.Interfaces
{
    public interface IProizvodService
    {
        List<ProizvodModel> GetAll(ProizvodParameters parameters);
        ProizvodModel GetByID(Guid id);
        ProizvodConfirmation Add(ProizvodModel proizvodModel);
        ProizvodConfirmation Update(ProizvodModel proizvodModel);
        void Delete(Guid proizvodID);
        List<ProizvodModel> GetByKorisnikID(Guid id);
        void ApplySort(ref IQueryable<ProizvodModel> proizvodi, string orderByQueryString);
        void SearchByNaziv(ref IQueryable<ProizvodModel> proizvodi, string proizvodNaziv);
        void SearchByVrsta(ref IQueryable<ProizvodModel> proizvodi, string proizvodVrsta);
        void SearchByPodVrsta(ref IQueryable<ProizvodModel> proizvodi, string proizvodPodVrsta);
        void SearchByKategorija(ref IQueryable<ProizvodModel> proizvodi, string proizvodKategorija);
        void SearchByKategorijaGodine(ref IQueryable<ProizvodModel> proizvodi, string proizvodKategorijaGodine);
        void SearchByKorisnickoIme(ref IQueryable<ProizvodModel> proizvodi, string proizvodKorisnickoIme);
        void SearchByVelicina(ref IQueryable<ProizvodModel> proizvodi, string proizvodVelicina);
    }
}
