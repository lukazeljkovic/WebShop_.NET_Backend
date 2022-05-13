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
    public class AdresaService : IAdresaService
    {

        private DataBaseContext _context;
        private readonly IMapper _mapper;

        public AdresaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public AdresaConfirmation Add(AdresaModel adresaModel)
        {
            _context.Add(adresaModel);

            _context.SaveChanges();
            return _mapper.Map<AdresaConfirmation>(adresaModel);
        }

        public void Delete(Guid adresaID)
        {
            _context.Remove(_context.Adrese.FirstOrDefault(adresa => adresa.AdresaID == adresaID));
            _context.SaveChanges();
        }

        public List<AdresaModel> GetAll(AdresaParameters parameters)
        {
            return _context.Adrese.
                 Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                    .Take(parameters.PageSize).ToList();
        }

        public AdresaModel GetByID(Guid id)
        {
            return _context.Adrese.FirstOrDefault(adresa => adresa.AdresaID == id);
        }

        public AdresaConfirmation Update(AdresaModel adresaModel)
        {
            AdresaModel adresa = GetByID(adresaModel.AdresaID);

            adresa.AdresaID = adresaModel.AdresaID;
            adresa.Broj = adresaModel.Broj;
            adresa.Grad = adresaModel.Grad;
            adresa.Ulica = adresaModel.Ulica;
            _context.SaveChanges();

            return new AdresaConfirmation
            {
                AdresaID = adresa.AdresaID,
                Broj = adresa.Broj,
                Ulica = adresa.Ulica
            };
        }

        public void ApplySort(ref IQueryable<AdresaModel> adrese, string orderByQueryString)
        {
            if (!adrese.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                adrese = adrese.OrderBy(x => x.Ulica);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(AdresaModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                adrese = adrese.OrderBy(x => x.Ulica);
                return;
            }
            adrese = adrese.OrderBy(orderQuery);
        }
    }
}
