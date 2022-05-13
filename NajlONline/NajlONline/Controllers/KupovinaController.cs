using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NajlONline.Models;
using NajlONline.Services;
using NajlONline.Services.Interfaces;
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
    [Route("api/kupovina")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class KupovinaController : Controller
    {
        private IKupovinaService _kupovina;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public KupovinaController(IKupovinaService kupovina, IMapper mapper, LinkGenerator linkGenerator)
        {
            _kupovina = kupovina;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<KupovinaDTO>> GetKupovine([FromQuery] KupovinaParameters parameters)
        {
            List<KupovinaModel> kupovine = _kupovina.GetAll(parameters);
            if (kupovine == null || kupovine.Count == 0)
            {
                return NoContent();
            }

            var kupovineQ = kupovine.AsQueryable();

            _kupovina.ApplySort(ref kupovineQ, parameters.OrderBy);

            kupovine = kupovineQ.ToList();

            return Ok(_mapper.Map<List<KupovinaDTO>>(kupovine));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        [HttpGet("{kupovinaID}")]
        public ActionResult<KupovinaDTO> GetKupovinaById(Guid kupovinaID)
        {
            KupovinaModel kupovinaModel = _kupovina.GetByID(kupovinaID);
            if (kupovinaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KupovinaDTO>(kupovinaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public ActionResult<KupovinaConfirmation> CreateKupovina([FromBody] KupovinaCreationDTO kupovina)
        {
            try
            {
                KupovinaModel kupovina2 = _mapper.Map<KupovinaModel>(kupovina);
                KupovinaConfirmation confirmation = _kupovina.Add(kupovina2);
                string location = _linkGenerator.GetPathByAction("GetKupovine", "Kupovina", new { kupovinaID = confirmation.KupovinaID });
                return Created(location, _mapper.Map<KupovinaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Kupovina uspešno obrisano</response>
        /// <response code="404">Nije pronađea kupovina za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja kupovine</response>
        [HttpDelete("{kupovinaId}")]
        [Authorize]
        public IActionResult DeleteKupovina(Guid kupovinaID)
        {
            try
            {
                KupovinaModel kupovina = _kupovina.GetByID(kupovinaID);

                if (kupovina == null)
                {
                    return NotFound();
                }

                _kupovina.Delete(kupovinaID);
                return NoContent();
            }

            catch (DbUpdateException)
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati kupovinu jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriranu kupovinu</response>
        /// <response code="400">Kupovina koja se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja kupovine</response>
        [HttpPut]
        [Authorize]
        public ActionResult<KupovinaConfirmation> UpdateKupovina(KupovinaUpdateDTO kupovina)
        {
            try
            {
                if (_kupovina.GetByID(kupovina.KupovinaID) == null)
                {
                    return NotFound();
                }

                KupovinaModel kupovinaModel = _mapper.Map<KupovinaModel>(kupovina);
                KupovinaConfirmation confirmation = _kupovina.Update(kupovinaModel);
                return Ok(_mapper.Map<KupovinaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("korisnik/{korisnikID}")]
        [Authorize]
        public ActionResult<KupovinaDTO> GetKupovinaByKorisnikId(Guid korisnikID)
        {
            List<KupovinaModel> kupovine = _kupovina.GetByKorisnikID(korisnikID);
            if (kupovine == null || kupovine.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<KupovinaDTO>>(kupovine));
        }
    }
}
