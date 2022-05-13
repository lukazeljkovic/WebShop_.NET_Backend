using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NajlONline.DTOs;
using NajlONline.DTOs.Confirmation;
using NajlONline.DTOs.Creation;
using NajlONline.Models;
using NajlONline.Services;
using NajlONlineData.DTOs;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineData.DTOs.Creation;
using NajlONlineData.DTOs.Update;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Controllers
{
    [Route("api/adresa")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class AdresaController : Controller
    {
        private IAdresaService _adresa;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public AdresaController(IAdresaService adresa, IMapper mapper, LinkGenerator linkGenerator)
        {
            _adresa = adresa;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<AdresaDTO>> GetAdrese([FromQuery] AdresaParameters parameters)
        {
            List<AdresaModel> adrese = _adresa.GetAll(parameters);
            var adreseQ = adrese.AsQueryable();
            if (adrese == null || adrese.Count == 0)
            {
                return NoContent();
            }
            _adresa.ApplySort(ref adreseQ, parameters.OrderBy);

            adrese = adreseQ.ToList();

            return Ok(_mapper.Map<List<AdresaDTO>>(adrese));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        [HttpGet("{adresaID}")]
        public ActionResult<AdresaDTO> GetAdresaById(Guid adresaID)
        {
            AdresaModel adresaModel = _adresa.GetByID(adresaID);
            if (adresaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AdresaDTO>(adresaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public ActionResult<AdresaConfirmation> CreateAdresa([FromBody] AdresaCreationDTO adresa)
        {
            try
            {
                AdresaModel adresa2 = _mapper.Map<AdresaModel>(adresa);
                AdresaConfirmation confirmation = _adresa.Add(adresa2);
                string location = _linkGenerator.GetPathByAction("GetAdrese", "Adresa", new { adresaID = confirmation.AdresaID });
                return Created(location, _mapper.Map<AdresaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Pravno lice uspešno obrisano</response>
        /// <response code="404">Nije pronađeno pravno lice za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja pravnog lica</response>
        [HttpDelete("{adresaId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAdresa(Guid adresaID)
        {
            try
            {
                AdresaModel adresa = _adresa.GetByID(adresaID);

                if (adresa == null)
                {
                    return NotFound();
                }

                _adresa.Delete(adresaID);
                return NoContent();
            }

            catch (DbUpdateException )
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati adresu jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriranu adresu</response>
        /// <response code="400">Adresa koja se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja adrese</response>
        [HttpPut]
        public ActionResult<AdresaConfirmation> UpdateAdresa(AdresaUpdateDTO adresa)
        {
            try
            {
                if (_adresa.GetByID(adresa.AdresaID) == null)
                {
                    return NotFound();
                }

                AdresaModel adresaModel = _mapper.Map<AdresaModel>(adresa);
                AdresaConfirmation confirmation = _adresa.Update(adresaModel);
                return Ok(_mapper.Map<AdresaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }
    }
}
