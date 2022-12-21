using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NajlONline.DTOs.Confirmation;
using NajlONline.Models;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace NajlONline.Services
{
    public class KorisnikService : IKorisnikService
    {
        private DataBaseContext _context;
        private readonly IMapper _mapper;

        public KorisnikService (DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public KorisnikConfirmation Add(KorisnikModel korisnikModel)
        {
            _context.Add(korisnikModel);

            _context.SaveChanges();
            return _mapper.Map<KorisnikConfirmation>(korisnikModel);

            // ako ovo radi dodati kod svih ostalih ovako

        }

        public List<KorisnikModel> GetAll(KorisnikParameters korisnikParameters)
        {
            return _context.Korisnici.Include(korisnik => korisnik.Uloga)
                                      .Include(korisnik => korisnik.Adresa).
                                      Skip((korisnikParameters.PageNumber - 1) * korisnikParameters.PageSize)
                                    .Take(korisnikParameters.PageSize)
                                    .ToList();
        }

        public KorisnikModel GetByID(Guid id)
        {
            return _context.Korisnici.Include(a => a.Uloga)
                                      .Include(a => a.Adresa)
                                       .FirstOrDefault(korisnik => korisnik.KorisnikID == id);
        }

        public KorisnikModel GetByKorisnickoIme(string korisnickoIme)
        {
            return _context.Korisnici.Include(a => a.Uloga)
                                      .Include(a => a.Adresa)
                                       .FirstOrDefault(korisnik => korisnik.KorisnickoIme == korisnickoIme);
        }



        public KorisnikConfirmation Update(KorisnikModel korisnikModel)
        {
            KorisnikModel korisnik = GetByID(korisnikModel.KorisnikID);

            korisnik.KorisnikID = korisnikModel.KorisnikID;
            korisnik.Ime = korisnikModel.Ime;
            korisnik.Prezime = korisnikModel.Prezime;
            korisnik.KorisnickoIme = korisnikModel.KorisnickoIme;
            korisnik.Lozinka = korisnikModel.Lozinka;
            korisnik.Telefon = korisnikModel.Telefon;
            //korisnik.Uloga = korisnikModel.Uloga;
            korisnik.UlogaID = korisnikModel.UlogaID;
            korisnik.Email = korisnikModel.Email;
            korisnik.AdresaID = korisnikModel.AdresaID;
           // korisnik.Adresa = korisnikModel.Adresa;
           
            _context.SaveChanges();
            return new KorisnikConfirmation
            {
                KorisnikID = korisnik.KorisnikID,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime
            };
        }

        public void Delete(Guid korisnikID)
        {
            _context.Remove(_context.Korisnici.FirstOrDefault(korisnik => korisnik.KorisnikID == korisnikID));
            _context.SaveChanges();
        }

        public Guid GetKorisnikIdByLozinkaIKorisnickoIme(string korisnickoIme, string lozinka)
        {
            return _context.Korisnici.FirstOrDefault(korisnik => korisnik.KorisnickoIme == korisnickoIme & korisnik.Lozinka == lozinka).KorisnikID;
        }

        public void SearchByKorisnickoIme(ref IQueryable<KorisnikModel> korisnici, string korisnickoIme)
        {
            if (!korisnici.Any() || string.IsNullOrWhiteSpace(korisnickoIme))
                return;
            korisnici = korisnici.Where(o => o.KorisnickoIme.ToLower().Contains(korisnickoIme.Trim().ToLower()));
        }

        public void ApplySort(ref IQueryable<KorisnikModel> korisnici, string orderByQueryString)
        {
            if (!korisnici.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                korisnici = korisnici.OrderBy(x => x.Ime);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(KorisnikModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                korisnici = korisnici.OrderBy(x => x.Ime);
                return;
            }
            korisnici = korisnici.OrderBy(orderQuery);
        }

        
    }
}
