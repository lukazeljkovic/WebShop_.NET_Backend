using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NajlONline.Models;
using NajlONline.Services.Interfaces;
using NajlONlineData.DTOs.Confirmation;
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
    public class ProizvodService : IProizvodService
    {
        private DataBaseContext _context;
        private readonly IMapper _mapper;

        public ProizvodService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ProizvodConfirmation Add(ProizvodModel proizvodModel)
        {
            _context.Add(proizvodModel);
            _context.SaveChanges();
            return _mapper.Map<ProizvodConfirmation>(proizvodModel);
        }

        public void Delete(Guid proizvodID)
        {
            _context.Remove(_context.Proizvodi.FirstOrDefault(proizvod => proizvod.ProizvodID == proizvodID));
            _context.SaveChanges();
        }

        public List<ProizvodModel> GetAll(ProizvodParameters parameters)
        {
            return _context.Proizvodi.Include(a => a.Kategorija)
                                      .Include(a=> a.Korisnik)
                                      .Include(a=>a.VrstaProizvoda)
                                      .Include(a=>a.Sezona)
                                      .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                    .Take(parameters.PageSize)
                                    .ToList();

           // var proizvodi = FindByCondition()
        }

        public ProizvodModel GetByID(Guid id)
        {
            return _context.Proizvodi.Include(a => a.Kategorija)
                                      .Include(a => a.Korisnik)
                                      .Include(a => a.VrstaProizvoda)
                                      .Include(a => a.Sezona)
                                      .FirstOrDefault(a=>a.ProizvodID == id);
        }

        public ProizvodConfirmation Update(ProizvodModel proizvodModel)
        {
            ProizvodModel proizvod = GetByID(proizvodModel.ProizvodID);

            proizvod.Cena = proizvodModel.Cena;
            proizvod.KategorijaID = proizvodModel.KategorijaID;
            proizvod.KorisnikID = proizvodModel.KorisnikID;
            proizvod.Naziv = proizvodModel.Naziv;
            proizvod.ProizvodID = proizvodModel.ProizvodID;
            proizvod.SezonaID = proizvodModel.SezonaID;
            proizvod.TrenutnoStanje = proizvodModel.TrenutnoStanje;
            proizvod.Velicina = proizvodModel.Velicina;
            proizvod.VrstaProizvodaID = proizvodModel.VrstaProizvodaID;
            
            _context.SaveChanges();

            return new ProizvodConfirmation
            {
                Cena = proizvod.Cena,
                Naziv = proizvod.Naziv,
                ProizvodID = proizvod.ProizvodID
            };
        }

        public List<ProizvodModel> GetByKorisnikID(Guid id)
        {
            if (id == Guid.Empty)
            {
                return _context.Proizvodi.Include(a => a.Kategorija)
                                          .Include(a => a.Korisnik)
                                          .Include(a => a.VrstaProizvoda)
                                          .Include(a => a.Sezona)
                                          .ToList();
            }
            else
            {
                string uloga = _context.Korisnici.Include(a => a.Uloga)
                                          .Include(a => a.Adresa)
                                           .FirstOrDefault(korisnik => korisnik.KorisnikID == id).Uloga.Naziv;

                if (uloga == "Prodavac")
                {
                    return _context.Proizvodi.Include(a => a.Kategorija)
                                              .Include(a => a.Korisnik)
                                              .Include(a => a.VrstaProizvoda)
                                              .Include(a => a.Sezona)
                                              .Where(a => a.KorisnikID == id)
                                              .ToList();
                }
                else
                {
                    return _context.Proizvodi.Include(a => a.Kategorija)
                                          .Include(a => a.Korisnik)
                                          .Include(a => a.VrstaProizvoda)
                                          .Include(a => a.Sezona)
                                          .ToList();
                }
            }
        }

        public List<ProizvodModel> GetByNOTKorisnikID(Guid id)
        {
            return _context.Proizvodi.Include(a => a.Kategorija)
                                      .Include(a => a.Korisnik)
                                      .Include(a => a.VrstaProizvoda)
                                      .Include(a => a.Sezona)
                                      .Where(a => a.KorisnikID != id)
                                      .ToList();
        }

        public void SearchByNaziv(ref IQueryable<ProizvodModel> proizvodi, string proizvodNaziv)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodNaziv))
                return;
            proizvodi = proizvodi.Where(o => o.Naziv.ToLower().Contains(proizvodNaziv.Trim().ToLower()));
        }

        public void SearchByVrsta(ref IQueryable<ProizvodModel> proizvodi, string proizvodVrsta)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodVrsta))
                return;
            proizvodi = proizvodi.Where(o => o.VrstaProizvoda.NazivVrsteProizvoda.ToLower().Contains(proizvodVrsta.Trim().ToLower()));
        }

        public void SearchByPodVrsta(ref IQueryable<ProizvodModel> proizvodi, string proizvodPodVrsta)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodPodVrsta))
                return;
            proizvodi = proizvodi.Where(o => o.VrstaProizvoda.NazivPodvrsteProizvoda.ToLower().Contains(proizvodPodVrsta.Trim().ToLower()));
        }

        public void SearchByKategorija(ref IQueryable<ProizvodModel> proizvodi, string proizvodKategorija)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodKategorija))
                return;
            proizvodi = proizvodi.Where(o => o.Kategorija.NazivKategorije.ToLower().Contains(proizvodKategorija.Trim().ToLower()));
        }

        public void SearchByKategorijaGodine(ref IQueryable<ProizvodModel> proizvodi, string proizvodKategorijaGodine)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodKategorijaGodine))
                return;
            proizvodi = proizvodi.Where(o => o.Kategorija.Godine.ToLower().Contains(proizvodKategorijaGodine.Trim().ToLower()));
        }

        public void SearchBySezona(ref IQueryable<ProizvodModel> proizvodi, string proizvodSezona)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodSezona))
                return;
            proizvodi = proizvodi.Where(o => o.Sezona.NazivSezone.ToLower().Contains(proizvodSezona.Trim().ToLower()));
        }

        public void SearchByKorisnickoIme(ref IQueryable<ProizvodModel> proizvodi, string proizvodKorisnickoIme)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodKorisnickoIme))
                return;
            proizvodi = proizvodi.Where(o => o.Korisnik.KorisnickoIme.ToLower().Contains(proizvodKorisnickoIme.Trim().ToLower()));
        }

        public void SearchByVelicina(ref IQueryable<ProizvodModel> proizvodi, string proizvodVelicina)
        {
            if (!proizvodi.Any() || string.IsNullOrWhiteSpace(proizvodVelicina))
                return;
            proizvodi = proizvodi.Where(o => o.Velicina.ToLower().Contains(proizvodVelicina.Trim().ToLower()));
        }


        public  void ApplySort(ref IQueryable<ProizvodModel> proizvodi, string orderByQueryString)
        {
            if (!proizvodi.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                proizvodi = proizvodi.OrderBy(x => x.Naziv);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(ProizvodModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                proizvodi = proizvodi.OrderBy(x => x.Naziv);
                return;
            }
            proizvodi = proizvodi.OrderBy(orderQuery);
        }
    }
}
