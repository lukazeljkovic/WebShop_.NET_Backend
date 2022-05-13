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
using NajlONline.Services.Interfaces;
using NajlONlineData;
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
    [Route("api/kategorija")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class KategorijaController : Controller
    {
        private IKategorijaService _kategorija;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public KategorijaController(IKategorijaService kategorija, IMapper mapper, LinkGenerator linkGenerator)
        {
            _kategorija = kategorija;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public ActionResult<List<KategorijaDTO>> GetKategorije([FromQuery] KategorijaParameters parameters)
        {
            List<KategorijaModel> kategorije = _kategorija.GetAll(parameters);
            var kategorijeQ = kategorije.AsQueryable();
            if (kategorije == null || kategorije.Count == 0)
            {
                return NoContent();
            }
            _kategorija.ApplySort(ref kategorijeQ, parameters.OrderBy);

            kategorije = kategorijeQ.ToList();

            return Ok(_mapper.Map<List<KategorijaDTO>>(kategorije));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{kategorijaID}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<KategorijaDTO> GetKategorijaById(Guid kategorijaID)
        {
            KategorijaModel kategorijaModel = _kategorija.GetByID(kategorijaID);
            if (kategorijaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KategorijaDTO>(kategorijaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public ActionResult<KategorijaConfirmation> CreateKategorija([FromBody] KategorijaCreationDTO kategorija)
        {
            try
            {
                KategorijaModel kategorija2 = _mapper.Map<KategorijaModel>(kategorija);
                KategorijaConfirmation confirmation = _kategorija.Add(kategorija2);
                string location = _linkGenerator.GetPathByAction("GetKategorije", "Kategorija", new { kategorijaID = confirmation.KategorijaID });
                return Created(location, _mapper.Map<KategorijaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Kategorija uspešno obrisana</response>
        /// <response code="404">Nije pronađena kategorija za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja kategorije</response>
        [HttpDelete("{kategorijaId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteKategorija(Guid kategorijaID)
        {
            try
            {
                KategorijaModel kategorija = _kategorija.GetByID(kategorijaID);

                if (kategorija == null)
                {
                    return NotFound();
                }

                _kategorija.Delete(kategorijaID);
                return NoContent();
            }

            catch (DbUpdateException )
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati kategoriju jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriranu kategoriju</response>
        /// <response code="400">Kategorija koja se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja kategorije</response>
        [HttpPut]
        [Authorize]
        public ActionResult<KategorijaConfirmation> UpdateKategorija(KategorijaUpdateDTO kategorija)
        {
            try
            {
                if (_kategorija.GetByID(kategorija.KategorijaID) == null)
                {
                    return NotFound();
                }

                KategorijaModel kategorijaModel = _mapper.Map<KategorijaModel>(kategorija);
                KategorijaConfirmation confirmation = _kategorija.Update(kategorijaModel);
                return Ok(_mapper.Map<KategorijaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }
    }
}
