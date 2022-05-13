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
    public class SezonaService : ISezonaService
    {
        private DataBaseContext _context;
        private readonly IMapper _mapper;

        public SezonaService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public SezonaConfirmation Add(SezonaModel sezonaModel)
        {
            _context.Add(sezonaModel);
            _context.SaveChanges();
            return _mapper.Map<SezonaConfirmation>(sezonaModel);
        }

        public List<SezonaModel> GetAll(SezonaParameters parameters)
        {
            return _context.Sezone
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                    .Take(parameters.PageSize).
                                    ToList();
        }

        public SezonaModel GetByID(Guid id)
        {
            return _context.Sezone.FirstOrDefault(sezona => sezona.SezonaID == id);
        }

        public SezonaConfirmation Update(SezonaModel sezonaModel)
        {
            SezonaModel sezona = GetByID(sezonaModel.SezonaID);

            sezona.SezonaID = sezonaModel.SezonaID;
            sezona.NazivSezone = sezonaModel.NazivSezone;
            sezona.Godina = sezonaModel.Godina;

            _context.SaveChanges();

            return new SezonaConfirmation
            {
                SezonaID = sezona.SezonaID,
                NazivSezone = sezona.NazivSezone
            };
        }

        public void Delete(Guid sezonaID)
        {
            _context.Remove(_context.Sezone.FirstOrDefault(sezona => sezona.SezonaID == sezonaID));
            _context.SaveChanges();
        }

        public void ApplySort(ref IQueryable<SezonaModel> sezone, string orderByQueryString)
        {
            if (!sezone.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                sezone = sezone.OrderBy(x => x.NazivSezone);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(SezonaModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                sezone = sezone.OrderBy(x => x.NazivSezone);
                return;
            }
            sezone = sezone.OrderBy(orderQuery);
        }
    }
}
