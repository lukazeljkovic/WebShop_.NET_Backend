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
    public class KupovineService : IKupovinaService
    {
        private DataBaseContext _context;
        private readonly IMapper _mapper;

        public KupovineService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public KupovinaConfirmation Add(KupovinaModel kupovinaModel)
        {
            _context.Add(kupovinaModel);
            _context.SaveChanges();

            return _mapper.Map<KupovinaConfirmation>(kupovinaModel);
        }

        public void Delete(Guid kupovinaID)
        {
            _context.Remove(_context.Kupovine.FirstOrDefault(kupovina => kupovina.KupovinaID == kupovinaID));
            _context.SaveChanges();
        }

        public List<KupovinaModel> GetAll(KupovinaParameters parameters)
        {
            return _context.Kupovine.Include(kupovina => kupovina.Korisnik)
                                    .Include(kupovina => kupovina.Proizvod)
                                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                    .Take(parameters.PageSize).ToList();
        }

        public KupovinaModel GetByID(Guid id)
        {
            return _context.Kupovine.Include(kupovina => kupovina.Korisnik)
                                    .Include(kupovina => kupovina.Proizvod)
                                    .FirstOrDefault(kupovina => kupovina.KupovinaID == id);
        }

        public void UpdateUspesnaKupovina ()
        {
            KupovinaModel model = _context.Kupovine.OrderByDescending(kupovina => kupovina.DatumKupovine).FirstOrDefault();
            model.UspesnaKupovina = true;
            Update(model);
        }

        public KupovinaConfirmation Update(KupovinaModel kupovinaModel)
        {
            KupovinaModel kupovina = GetByID(kupovinaModel.KupovinaID);

            kupovina.DatumKupovine = kupovinaModel.DatumKupovine;
            kupovina.Kolicina = kupovinaModel.Kolicina;
            kupovina.KorisnikID = kupovinaModel.KorisnikID;
            kupovina.KupovinaID = kupovinaModel.KupovinaID;
            kupovina.ProizvodDostavljen = kupovinaModel.ProizvodDostavljen;
            kupovina.ProizvodID = kupovinaModel.ProizvodID;
            kupovina.ProizvodPorucen = kupovinaModel.ProizvodPorucen;
            kupovina.UspesnaKupovina = kupovinaModel.UspesnaKupovina;
            

            _context.SaveChanges();

            return new KupovinaConfirmation
            {
                DatumKupovine = kupovina.DatumKupovine,
                KupovinaID = kupovina.KupovinaID,
                ProizvodPorucen = kupovina.ProizvodPorucen
            };
        }

        public List<KupovinaModel> GetByKorisnikID(Guid id)
        {
            return _context.Kupovine.Include(kupovina => kupovina.Korisnik)
                                    .Include(kupovina => kupovina.Proizvod)
                                    .Where(kupovina => kupovina.KupovinaID == id)
                                    .ToList();
        }

        public void ApplySort(ref IQueryable<KupovinaModel> kupovine, string orderByQueryString)
        {
            if (!kupovine.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                kupovine = kupovine.OrderBy(x => x.DatumKupovine);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(KupovinaModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                kupovine = kupovine.OrderBy(x => x.DatumKupovine);
                return;
            }
            kupovine = kupovine.OrderBy(orderQuery);
        }
    }
}
