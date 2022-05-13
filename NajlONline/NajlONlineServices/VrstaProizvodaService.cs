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
    public class VrstaProizvodaService : IVrstaProizvodaService
    {

        private DataBaseContext _context;
        private readonly IMapper _mapper;
        public VrstaProizvodaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public VrstaProizvodaConfirmation Add(VrstaProizvodaModel vrstaProizvodaModel)
        {
            _context.Add(vrstaProizvodaModel);
            _context.SaveChanges();
            return _mapper.Map<VrstaProizvodaConfirmation>(vrstaProizvodaModel);
        }

        public List<VrstaProizvodaModel> GetAll(VrstaProizvodaParameters parameters)
        {
            return _context.VrsteProizvoda
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                    .Take(parameters.PageSize)
                                    .ToList();
        }

        public VrstaProizvodaModel GetByID(Guid id)
        {
            return _context.VrsteProizvoda.FirstOrDefault(vrsta => vrsta.VrstaProizvodaID == id);
        }

        public VrstaProizvodaConfirmation Update(VrstaProizvodaModel vrstaProizvodaModel)
        {
            VrstaProizvodaModel vrsta = GetByID(vrstaProizvodaModel.VrstaProizvodaID);

            vrsta.NazivPodvrsteProizvoda = vrstaProizvodaModel.NazivPodvrsteProizvoda;
            vrsta.NazivVrsteProizvoda = vrstaProizvodaModel.NazivVrsteProizvoda;
            vrsta.VrstaProizvodaID = vrstaProizvodaModel.VrstaProizvodaID;
            _context.SaveChanges();

            return new VrstaProizvodaConfirmation
            {
                NazivVrsteProizvoda = vrsta.NazivVrsteProizvoda,
                VrstaProizvodaID = vrsta.VrstaProizvodaID
            };
        }

        public void Delete(Guid vrstaProizvodaID)
        {
            _context.Remove(_context.VrsteProizvoda.FirstOrDefault(vrsta => vrsta.VrstaProizvodaID == vrstaProizvodaID));
            _context.SaveChanges();
        }

        public void ApplySort(ref IQueryable<VrstaProizvodaModel> vrsteProizvoda, string orderByQueryString)
        {
            if (!vrsteProizvoda.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                vrsteProizvoda = vrsteProizvoda.OrderBy(x => x.NazivVrsteProizvoda);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(VrstaProizvodaModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                vrsteProizvoda = vrsteProizvoda.OrderBy(x => x.NazivVrsteProizvoda);
                return;
            }
            vrsteProizvoda = vrsteProizvoda.OrderBy(orderQuery);
        }
    }
}
