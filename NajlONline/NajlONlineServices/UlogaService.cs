using AutoMapper;
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
using Microsoft.EntityFrameworkCore;

namespace NajlONline.Services
{
    public class UlogaService : IUlogaService
    {
        private DataBaseContext _context;
        private readonly IMapper _mapper;
        public UlogaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       

        public UlogaConfirmation Add(UlogaModel ulogaModel)
        {
            _context.Add(ulogaModel);
            _context.SaveChanges();
            return _mapper.Map<UlogaConfirmation>(ulogaModel);
        }

        public List<UlogaModel> GetAll(UlogaParameters parameters)
        {
            return _context.Uloge
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                    .Take(parameters.PageSize).ToList();
        }

        public UlogaModel GetByID(Guid id)
        {
            return _context.Uloge.FirstOrDefault(uloga => uloga.UlogaID == id);
        }

        public string GetByKorisnikID(Guid id)
        {
            
            KorisnikModel korisnik = _context.Korisnici.Include(a => a.Uloga)
                .FirstOrDefault(korisnik => korisnik.KorisnikID == id);
            return korisnik.Uloga.Naziv;
        }

        public UlogaConfirmation Update(UlogaModel ulogaModel)
        {
            UlogaModel uloga = GetByID(ulogaModel.UlogaID);

            uloga.UlogaID = ulogaModel.UlogaID;
            uloga.Naziv = ulogaModel.Naziv;

            _context.SaveChanges();

            return new UlogaConfirmation
            {
                Naziv = uloga.Naziv,
                UlogaID = uloga.UlogaID
            };
        }

        public void Delete(Guid ulogaID)
        {
            _context.Remove(_context.Uloge.FirstOrDefault(uloga => uloga.UlogaID == ulogaID));
            _context.SaveChanges();
        }

        public void ApplySort(ref IQueryable<UlogaModel> uloge, string orderByQueryString)
        {
            if (!uloge.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                uloge = uloge.OrderBy(x => x.Naziv);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(UlogaModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                uloge = uloge.OrderBy(x => x.Naziv);
                return;
            }
            uloge = uloge.OrderBy(orderQuery);
        }
    }
}
