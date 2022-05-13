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
    [Route("api/vrstaProizvoda")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class VrstaProizvodaController : Controller
    {
        private IVrstaProizvodaService _vrstaProizvoda;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public VrstaProizvodaController(IVrstaProizvodaService vrstaProizvoda, IMapper mapper, LinkGenerator linkGenerator)
        {
            _vrstaProizvoda = vrstaProizvoda;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public ActionResult<List<VrstaProizvodaDTO>> GetVrsteProizvoda([FromQuery] VrstaProizvodaParameters parameters)
        {
            List<VrstaProizvodaModel> vrste = _vrstaProizvoda.GetAll(parameters);
            if (vrste == null || vrste.Count == 0)
            {
                return NoContent();
            }
            var vrsteQ = vrste.AsQueryable();

            _vrstaProizvoda.ApplySort(ref vrsteQ, parameters.OrderBy);

            vrste = vrsteQ.ToList();

            return Ok(_mapper.Map<List<VrstaProizvodaDTO>>(vrste));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{vrstaProizvodaID}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<VrstaProizvodaDTO> GetVrstaProizvodaById(Guid vrstaProizvodaID)
        {
            VrstaProizvodaModel vrstaProizvodaModel = _vrstaProizvoda.GetByID(vrstaProizvodaID);
            if (vrstaProizvodaModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VrstaProizvodaDTO>(vrstaProizvodaModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public ActionResult<VrstaProizvodaConfirmation> CreateVrstaProizvoda([FromBody] VrstaProizvodaCreationDTO vrsta)
        {
            try
            {
                VrstaProizvodaModel vrsta2 = _mapper.Map<VrstaProizvodaModel>(vrsta);
                VrstaProizvodaConfirmation confirmation = _vrstaProizvoda.Add(vrsta2);
                string location = _linkGenerator.GetPathByAction("GetVrsteProizvoda", "VrstaProizvoda", new { vrstaProizvodaID = confirmation.VrstaProizvodaID });
                return Created(location, _mapper.Map<VrstaProizvodaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Vrsta Proizvoda uspešno obrisana</response>
        /// <response code="404">Nije pronađena Vrsta Proizvoda za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja Vrsta Proizvoda</response>
        [HttpDelete("{vrstaProizvodaId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVrstaProizvoda(Guid vrstaProizvodaID)
        {
            try
            {
                VrstaProizvodaModel vrsta = _vrstaProizvoda.GetByID(vrstaProizvodaID);

                if (vrsta == null)
                {
                    return NotFound();
                }

                _vrstaProizvoda.Delete(vrstaProizvodaID);
                return NoContent();
            }

            catch (DbUpdateException)
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati vrstu proizvoda jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriranu vrstu proizvoda</response>
        /// <response code="400">Vrsta proizvoda koja se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja vrste proizvoda</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<VrstaProizvodaConfirmation> UpdateVrstaProizvoda(VrstaProizvodaUpdateDTO vrsta)
        {
            try
            {
                if (_vrstaProizvoda.GetByID(vrsta.VrstaProizvodaID) == null)
                {
                    return NotFound();
                }

                VrstaProizvodaModel vrstaProizvodaModel = _mapper.Map<VrstaProizvodaModel>(vrsta);
                VrstaProizvodaConfirmation confirmation = _vrstaProizvoda.Update(vrstaProizvodaModel);
                return Ok(_mapper.Map<VrstaProizvodaConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }
    }
}
