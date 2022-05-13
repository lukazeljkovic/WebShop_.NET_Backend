using AutoMapper;
using Microsoft.EntityFrameworkCore;

using NajlONline.Models;
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
    public class KarticaService : IKarticaService
    {
        private DataBaseContext _context;
        private readonly IMapper _mapper;

        public KarticaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public KarticaConfirmation Add(KarticaModel karticaModel)
        {
            _context.Add(karticaModel);
            _context.SaveChanges();
            return _mapper.Map<KarticaConfirmation>(karticaModel);
        }

        public void Delete(Guid karticaID)
        {
            _context.Remove(_context.Kartice.FirstOrDefault(kartica => kartica.KarticaID == karticaID));
            _context.SaveChanges();
        }

        public List<KarticaModel> GetAll(KarticaParameters parameters)
        {
            return _context.Kartice.Include(a => a.Korisnik)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize).ToList();
        }

        public KarticaModel GetByID(Guid id)
        {
            return _context.Kartice.Include(a => a.Korisnik)
                                       .FirstOrDefault(kartica => kartica.KarticaID == id);
        }

        public KarticaConfirmation Update(KarticaModel karticaModel)
        {
            KarticaModel kartica = GetByID(karticaModel.KarticaID);

            kartica.KarticaID = karticaModel.KarticaID;
            kartica.BrojRacuna = karticaModel.BrojRacuna;
            kartica.CVC = karticaModel.CVC;
            kartica.DatumIsteka = karticaModel.DatumIsteka;
            kartica.KorisnikID = karticaModel.KorisnikID;
            _context.SaveChanges();

            return new KarticaConfirmation
            {
                KarticaID = kartica.KarticaID,
                BrojRacuna = kartica.BrojRacuna
            };
        }

        public void ApplySort(ref IQueryable<KarticaModel> kartice, string orderByQueryString)
        {
            if (!kartice.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                kartice = kartice.OrderBy(x => x.DatumIsteka);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(KarticaModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                kartice = kartice.OrderBy(x => x.DatumIsteka);
                return;
            }
            kartice = kartice.OrderBy(orderQuery);
        }
    }
}
