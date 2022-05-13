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
    [Route("api/uloga")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class UlogaController : Controller
    {
        private IUlogaService _uloga;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public UlogaController(IUlogaService uloga, IMapper mapper, LinkGenerator linkGenerator)
        {
            _uloga = uloga;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<UlogaDTO>> GetUloge([FromQuery] UlogaParameters parameters)
        {
            List<UlogaModel> uloge = _uloga.GetAll(parameters);
            if (uloge == null || uloge.Count == 0)
            {
                return NoContent();
            }

            var ulogeQ = uloge.AsQueryable();

            _uloga.ApplySort(ref ulogeQ, parameters.OrderBy);

            uloge = ulogeQ.ToList();

            return Ok(_mapper.Map<List<UlogaDTO>>(uloge));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        [HttpGet("{ulogaID}")]
        public ActionResult<UlogaDTO> GetUlogaById(Guid ulogaID)
        {
            UlogaModel ulogaModel = _uloga.GetByID(ulogaID);
            if (ulogaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UlogaDTO>(ulogaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UlogaConfirmation> CreateUloga([FromBody] UlogaCreationDTO uloga)
        {
            try
            {
                UlogaModel uloga2 = _mapper.Map<UlogaModel>(uloga);
                UlogaConfirmation confirmation = _uloga.Add(uloga2);
                string location = _linkGenerator.GetPathByAction("GetUloge", "Uloga", new { ulogaID = confirmation.UlogaID });
                return Created(location, _mapper.Map<UlogaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Uloga uspešno obrisana</response>
        /// <response code="404">Nije pronađena uloga za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja uloge</response>
        [HttpDelete("{ulogaId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUloga(Guid ulogaID)
        {
            try
            {
                UlogaModel uloga = _uloga.GetByID(ulogaID);

                if (uloga == null)
                {
                    return NotFound();
                }

                _uloga.Delete(ulogaID);
                return NoContent();
            }

            catch (DbUpdateException)
            {
                
                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati ulogu jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error " + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriranu ulogu</response>
        /// <response code="400">Uloga koja se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja uloge</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<UlogaConfirmation> UpdateUloga(UlogaUpdateDTO uloga)
        {
            try
            {
                if (_uloga.GetByID(uloga.UlogaID) == null)
                {
                    return NotFound();
                }

                UlogaModel ulogaModel = _mapper.Map<UlogaModel>(uloga);
                UlogaConfirmation confirmation = _uloga.Update(ulogaModel);
                return Ok(_mapper.Map<UlogaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }
    }
}
