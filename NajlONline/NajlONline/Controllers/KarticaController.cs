using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
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
    [Route("api/kartica")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class KarticaController : Controller
    {
        private IKarticaService _kartica;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public KarticaController(IKarticaService kartica, IMapper mapper, LinkGenerator linkGenerator)
        {
            _kartica = kartica;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<KarticaDTO>> GetKartice([FromQuery] KarticaParameters parameters)
        {
            List<KarticaModel> kartice = _kartica.GetAll(parameters);
            var karticeQ = kartice.AsQueryable();
            if (kartice == null || kartice.Count == 0)
            {
                return NoContent();
            }
            _kartica.ApplySort(ref karticeQ, parameters.OrderBy);

            kartice = karticeQ.ToList();

            return Ok(_mapper.Map<List<KarticaDTO>>(kartice));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{karticaID}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<KarticaDTO> GetKarticaById(Guid karticaID)
        {
            KarticaModel karticaModel = _kartica.GetByID(karticaID);
            if (karticaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KarticaDTO>(karticaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public ActionResult<KarticaConfirmation> CreateKartica([FromBody] KarticaCreationDTO kartica)
        {
            try
            {
                KarticaModel kartica2 = _mapper.Map<KarticaModel>(kartica);
                KarticaConfirmation confirmation = _kartica.Add(kartica2);
                string location = _linkGenerator.GetPathByAction("GetKartice", "Kartica", new { karticaID = confirmation.KarticaID });
                return Created(location, _mapper.Map<KarticaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Kartica uspešno obrisana</response>
        /// <response code="404">Nije pronađena kartica za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja kartice</response>
        [HttpDelete("{karticaId}")]
        [Authorize]
        public IActionResult DeleteKartica(Guid karticaID)
        {
            try
            {
                KarticaModel kartica = _kartica.GetByID(karticaID);

                if (kartica == null)
                {
                    return NotFound();
                }

                _kartica.Delete(karticaID);
                return NoContent();
            }

            catch (DbUpdateException )
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati karticu jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriranu karticu</response>
        /// <response code="400">Kartica koja se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja kartice</response>
        [HttpPut]
        [Authorize]
        public ActionResult<KarticaConfirmation> UpdateKartica(KarticaUpdateDTO kartica)
        {
            try
            {
                if (_kartica.GetByID(kartica.KarticaID) == null)
                {
                    return NotFound();
                }

                KarticaModel karticaModel = _mapper.Map<KarticaModel>(kartica);
                KarticaConfirmation confirmation = _kartica.Update(karticaModel);
                return Ok(_mapper.Map<KarticaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }
    }
}
