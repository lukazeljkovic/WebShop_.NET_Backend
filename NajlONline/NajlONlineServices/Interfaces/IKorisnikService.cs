using NajlONline.DTOs.Confirmation;
using NajlONline.Models;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Services
{
    public interface IKorisnikService
    {
        List<KorisnikModel> GetAll(KorisnikParameters korisnikParameters);
        KorisnikModel GetByID(Guid id);
        KorisnikConfirmation Add(KorisnikModel korisnikModel);
        KorisnikConfirmation Update(KorisnikModel korisnikModel);
        void Delete(Guid korisnikID);
        KorisnikModel GetByKorisnickoIme(string korisnickoIme);
        void ApplySort(ref IQueryable<KorisnikModel> korisnici, string orderByQueryString);
        void SearchByKorisnickoIme(ref IQueryable<KorisnikModel> korisnici, string korisnickoIme);
    }
}
