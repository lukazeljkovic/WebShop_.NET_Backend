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
using NajlONlineData.DTOs.Update;
using NajlONlineServices.Interfaces;
using NajlONlineServices.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NajlONline.Controllers
{
    [Route("api/korisnik")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class KorisnikController : Controller
    {
        private IKorisnikService _korisnik;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        private readonly IJWTAuth _jwtAuth;

        public KorisnikController(IKorisnikService korisnik, IMapper mapper, LinkGenerator linkGenerator, IJWTAuth jwtAuth)
        {
            _korisnik = korisnik;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _jwtAuth = jwtAuth;
        }

        [HttpGet]
        [HttpHead]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<KorisnikDTO>> GetKorisnici([FromQuery] KorisnikParameters parameters)
        {
            List<KorisnikModel> korisnici = _korisnik.GetAll(parameters);
            
            if (korisnici == null || korisnici.Count == 0)
            {
                return NoContent();
            }
            var korisniciQ = korisnici.AsQueryable();

            _korisnik.ApplySort(ref korisniciQ, parameters.OrderBy);

            _korisnik.SearchByKorisnickoIme(ref korisniciQ, parameters.KorisnickoIme);

            korisnici = korisniciQ.ToList();

            return Ok(_mapper.Map<List<KorisnikDTO>>(korisnici));
        }

        

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{korisnickoIme}")]
        public ActionResult<KorisnikDTO> GetKorisnikByKorisnickoIme(string korisnickoIme)
        {
            KorisnikModel korisnikModel = _korisnik.GetByKorisnickoIme(korisnickoIme);
            if (korisnikModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<KorisnikDTO>(korisnikModel));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikConfirmation> CreateKorisnik([FromBody] KorisnikCreationDTO korisnik)
        {
            try
            {
                KorisnikModel korisnik2 = _mapper.Map<KorisnikModel>(korisnik);
                KorisnikConfirmation confirmation = _korisnik.Add(korisnik2);
                string location = _linkGenerator.GetPathByAction("GetKorisnici", "Korisnik", new { korisnikID = confirmation.KorisnikID });
                return Created(location ,_mapper.Map<KorisnikConfirmationDTO>(confirmation));
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
        [HttpDelete("{korisnikId}")]
        [Authorize]
        public IActionResult DeleteKorisnik(Guid korisnikID)
        {
            try
            {
                KorisnikModel korisnik = _korisnik.GetByID(korisnikID);

                if (korisnik == null)
                {
                    return NotFound();
                }

                _korisnik.Delete(korisnikID);
                return NoContent();
            }

            catch (DbUpdateException)
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Ne mozete izbrisati korisnika jer se koristi na drugim mestima");
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error" + ex.Message);
            }
        }

        
        ///  <response code="200">Vraća ažurirano pravno lice</response>
        /// <response code="400">Pravno lice koje se ažurira nije pronađena</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja pravnog lica</response>
        [HttpPut]
        [Authorize]
        public ActionResult<KorisnikConfirmation> UpdateKorisnik(KorisnikUpdateDTO korisnik)
        {
            try
            {
                if (_korisnik.GetByID(korisnik.KorisnikID) == null)
                {
                    return NotFound();
                }

                KorisnikModel korisnikModel = _mapper.Map<KorisnikModel>(korisnik);
                KorisnikConfirmation confirmation = _korisnik.Update(korisnikModel);
                return Ok(_mapper.Map<KorisnikConfirmationDTO>(confirmation));
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error" + ex.InnerException.Message);
            }
        }

        //[AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("login")]
        public IActionResult Authentication([FromBody] KorisnikLogInDTO korisnik)
        {
            KorisnikModel korisnikModel = _korisnik.GetByKorisnickoIme(korisnik.KorisnickoIme);
            if (korisnikModel == null || korisnikModel.Lozinka != korisnik.Lozinka)
            {
                return Unauthorized();
            }
            var token = _jwtAuth.Authentication(korisnik.KorisnickoIme, korisnik.Lozinka);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }

    }
}
