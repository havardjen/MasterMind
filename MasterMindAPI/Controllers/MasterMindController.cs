using MasterMindResources.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasterMindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterMindController : ControllerBase
    {
        public MasterMindController(ICharactersService charService)
        {
            _charactersService = charService;
        }

        private readonly ICharactersService _charactersService;

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Welcome to this super incredible amazing relatively ok MasterMind service");
        }

        [HttpGet, Route("ValidCharacters")]
        public ActionResult<List<string>> GetValidCharacters()
        {
            var validCharacters = _charactersService.GetCharacter(string.Empty, true);

            if (validCharacters == null || validCharacters.Count == 0)
            {
                return NotFound("No valid characters found.");
            }
            return Ok(validCharacters);
        }
    }
}
