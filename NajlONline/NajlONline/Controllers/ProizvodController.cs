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
    [Route("api/proizvod")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class ProizvodController : Controller
    {
        private IProizvodService _proizvod;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ProizvodController(IProizvodService proizvod, IMapper mapper, LinkGenerator linkGenerator)
        {
            _proizvod = proizvod;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
       // [Authorize(Roles = "Admin")]
        public ActionResult<List<ProizvodDTO>> GetProizvodi([FromQuery] ProizvodParameters parameters)
        {
            List<ProizvodModel> proizvodi = _proizvod.GetAll(parameters);
            var proizvodiQ = proizvodi.AsQueryable();
            if (proizvodi == null || proizvodi.Count == 0)
            {
                return NoContent();
            }
            _proizvod.ApplySort(ref proizvodiQ, parameters.OrderBy);

            _proizvod.SearchByNaziv(ref proizvodiQ, parameters.Naziv);

            _proizvod.SearchByVrsta(ref proizvodiQ, parameters.Vrsta);

            _proizvod.SearchByPodVrsta(ref proizvodiQ, parameters.PodVrsta);

            _proizvod.SearchByKategorija(ref proizvodiQ, parameters.Kategorija);

            _proizvod.SearchByKorisnickoIme(ref proizvodiQ, parameters.KorisnickoIme);

            _proizvod.SearchByVelicina(ref proizvodiQ, parameters.Velicina);

            proizvodi = proizvodiQ.ToList();

            return Ok(_mapper.Map<List<ProizvodDTO>>(proizvodi));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{proizvodID}")]
        //[Authorize(Roles = "Admin")]
        public ActionResult<ProizvodDTO> GetProizvodById(Guid proizvodID)
        {
            ProizvodModel proizvodModel = _proizvod.GetByID(proizvodID);
            if (proizvodModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProizvodDTO>(proizvodModel));
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("korisnik/{korisnikID}")]
       // [Authorize]
        public ActionResult<ProizvodDTO> GetProizvodByKorisnikId(Guid korisnikID)
        {
            List<ProizvodModel> proizvodi = _proizvod.GetByKorisnikID(korisnikID);
            if (proizvodi == null || proizvodi.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<ProizvodDTO>>(proizvodi));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("home/{korisnikID}")]
        // [Authorize]
        public ActionResult<ProizvodDTO> GetProizvodByNOTKorisnikId(Guid korisnikID)
        {
            List<ProizvodModel> proizvodi = _proizvod.GetByNOTKorisnikID(korisnikID);
            if (proizvodi == null || proizvodi.Count == 0)
            {
                return NoContent();
            }

            return Ok(_mapper.Map<List<ProizvodDTO>>(proizvodi));
        }

        [HttpPost]
        [Consumes("application/json")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProizvodConfirmation> CreateProizvod([FromBody] ProizvodCreationDTO proizvod)
        {
            try
            {
                ProizvodModel proizvod2 = _mapper.Map<ProizvodModel>(proizvod);
                ProizvodConfirmation confirmation = _proizvod.Add(proizvod2);
                string location = _linkGenerator.GetPathByAction("GetProizvodi", "Proizvod", new { proizvodID = confirmation.ProizvodID });
                return Created(location, _mapper.Map<ProizvodConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error " + ex.InnerException.Message);
            }
        }

        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Proizvod uspešno obrisan</response>
        /// <response code="404">Nije pronađen proizvod za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja proizvoda</response>
        [HttpDelete("{proizvodId}")]
        //[Authorize]
        public IActionResult DeleteProizvod(Guid proizvodID)
        {
            try
            {
                ProizvodModel proizvod = _proizvod.GetByID(proizvodID);

                if (proizvod == null)
                {
                    return NotFound();
                }

                _proizvod.Delete(proizvodID);
                return NoContent();
            }

            catch (DbUpdateException)
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati proizvod jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }


        ///  <response code="200">Vraća ažuriran proizvod</response>
        /// <response code="400">Proizvod koji se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja proizvoda</response>
        [HttpPut]
       // [Authorize]
        public ActionResult<ProizvodConfirmation> UpdateProizvod(ProizvodUpdateDTO proizvod)
        {
            try
            {
                if (_proizvod.GetByID(proizvod.ProizvodID) == null)
                {
                    return NotFound();
                }

                ProizvodModel proizvodModel = _mapper.Map<ProizvodModel>(proizvod);
                ProizvodConfirmation confirmation = _proizvod.Update(proizvodModel);
                return Ok(_mapper.Map<ProizvodConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }
    }
}
