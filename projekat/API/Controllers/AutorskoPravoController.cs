using Business.Abstractions;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorskoPravoController : ControllerBase
    {
        private readonly IPrijavaService _prijavaService;

        public AutorskoPravoController(IPrijavaService prijavaService)
        {
            _prijavaService = prijavaService;
        }

        [HttpPost("dodajPrijavu")]
        public IActionResult DodajPrijavu(AutorskoPravoModel model)
        {
            return Ok(_prijavaService.DodajPrijavu(model));
        }

        [HttpPost("pretragaMetapodaci")]
        public IActionResult PretraziPoMetapodacima(AutorskoPravoModel model)
        {
            return Ok(_prijavaService.PretraziPoMetapodacima(model));
        }

        [HttpPost("podnesiResenje")]
        public IActionResult PodnesiResenje(ResenjeModel model)
        {
            return Ok(_prijavaService.PodnesiResenje(model));
        }

        [HttpGet("vratiSvePrijave")]
        
        public IActionResult VratiSvePrijave(bool isUserAdmin, string userName)
        {
            return Ok(_prijavaService.VratiSvePrijave(isUserAdmin, userName));
        }

        [HttpGet("vratiIzvestaj")]
        public IActionResult VratiSvePrijave(string start, string end)
        {
            return Ok(_prijavaService.VratiIzvestaj(start, end));
        }


        [HttpGet("pretraziPrijave")]
        public IActionResult PretraziPrijave(string pojam, bool isUserAdmin, string userName)
        {
            return Ok(_prijavaService.PretraziPrijave(pojam, isUserAdmin, userName));
        }

        [HttpGet("vratiPrijavu")]
        public IActionResult VratiPrijavu(int id)
        {
            return Ok(_prijavaService.VratiPrijavu(id));
        }
    }
}
