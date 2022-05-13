using AutoMapper;
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

    public class KategorijaService : IKategorijaService
    {

        private DataBaseContext _context;
        private readonly IMapper _mapper;

        public KategorijaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public KategorijaConfirmation Add(KategorijaModel kategorijaModel)
        {
            _context.Add(kategorijaModel);

            _context.SaveChanges();

            return _mapper.Map<KategorijaConfirmation>(kategorijaModel);
        }

        public List<KategorijaModel> GetAll(KategorijaParameters parameters)
        {
            return _context.Kategorije
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize).ToList();
        }

        public KategorijaModel GetByID(Guid id)
        {
            return _context.Kategorije.FirstOrDefault(kategorija => kategorija.KategorijaID == id);
        }

        public KategorijaConfirmation Update(KategorijaModel kategorijaModel)
        {
            KategorijaModel kategorija = GetByID(kategorijaModel.KategorijaID);

            kategorija.Godine = kategorijaModel.Godine;
            kategorija.KategorijaID = kategorijaModel.KategorijaID;
            kategorija.NazivKategorije = kategorijaModel.NazivKategorije;

            _context.SaveChanges();

            return new KategorijaConfirmation
            {
                KategorijaID = kategorija.KategorijaID,
                NazivKategorije = kategorija.NazivKategorije
            };
        }

        public void Delete(Guid kategorijaID)
        {
            _context.Remove(_context.Kategorije.FirstOrDefault(kategorija => kategorija.KategorijaID == kategorijaID));
            _context.SaveChanges();
        }

        public void ApplySort(ref IQueryable<KategorijaModel> kategorije, string orderByQueryString)
        {
            if (!kategorije.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                kategorije = kategorije.OrderBy(x => x.NazivKategorije);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(KategorijaModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                kategorije = kategorije.OrderBy(x => x.NazivKategorije);
                return;
            }
            kategorije = kategorije.OrderBy(orderQuery);
        }
    }
}
