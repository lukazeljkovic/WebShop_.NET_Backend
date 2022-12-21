using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NajlONline.Models;
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
    [Route("api/sezona")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class SezonaController : Controller
    {
        private ISezonaService _sezona;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public SezonaController(ISezonaService sezona, IMapper mapper, LinkGenerator linkGenerator)
        {
            _sezona = sezona;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
       // [Authorize]
        public ActionResult<List<SezonaDTO>> GetSezone([FromQuery] SezonaParameters parameters)
        {
            List<SezonaModel> sezone = _sezona.GetAll(parameters);
            if (sezone == null || sezone.Count == 0)
            {
                return NoContent();
            }

            var sezoneQ = sezone.AsQueryable();

            _sezona.ApplySort(ref sezoneQ, parameters.OrderBy);

           sezone = sezoneQ.ToList();

            return Ok(_mapper.Map<List<SezonaDTO>>(sezone));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{sezonaID}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<SezonaDTO> GetSezonaById(Guid SezonaID)
        {
            SezonaModel sezonaModel = _sezona.GetByID(SezonaID);
            if (sezonaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SezonaDTO>(sezonaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public ActionResult<SezonaConfirmation> CreateSezona([FromBody] SezonaCreationDTO sezona)
        {
            try
            {
                SezonaModel sezona2 = _mapper.Map<SezonaModel>(sezona);
                SezonaConfirmation confirmation = _sezona.Add(sezona2);
                string location = _linkGenerator.GetPathByAction("GetSezone", "Sezona", new { sezonaID = confirmation.SezonaID });
                return Created(location, _mapper.Map<SezonaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Sezona uspešno obrisana</response>
        /// <response code="404">Nije pronađena sezona za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja sezone</response>
        [HttpDelete("{sezonaId}")]
        [Authorize]
        public IActionResult DeleteSezona(Guid sezonaID)
        {
            try
            {
                SezonaModel sezona = _sezona.GetByID(sezonaID);

                if (sezona == null)
                {
                    return NotFound();
                }

                _sezona.Delete(sezonaID);
                return NoContent();
            }

            catch (DbUpdateException)
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati sezonu jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriranu sezonu</response>
        /// <response code="400">Sezona koja se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja sezone</response>
        [HttpPut]
        [Authorize]
        public ActionResult<SezonaConfirmation> UpdateSezona(SezonaUpdateDTO sezona)
        {
            try
            {
                if (_sezona.GetByID(sezona.SezonaID) == null)
                {
                    return NotFound();
                }

                SezonaModel sezonaModel = _mapper.Map<SezonaModel>(sezona);
                SezonaConfirmation confirmation = _sezona.Update(sezonaModel);
                return Ok(_mapper.Map<SezonaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }
    }
}
