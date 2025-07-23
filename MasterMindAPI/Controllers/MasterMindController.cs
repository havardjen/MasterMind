using MasterMindResources.Interfaces;
using MasterMindResources.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasterMindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterMindController : ControllerBase
    {
        public MasterMindController(ICharactersService charService, IGameRepository gameRepository)
        {
            _charactersService = charService;
            _gameRepository = gameRepository;
        }

        private readonly ICharactersService _charactersService;
        private readonly IGameRepository _gameRepository;

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

        [HttpGet, Route("Game/Create")]
        public ActionResult<int> CreateGame()
        {
            return Ok(_gameRepository.CreateGame());
        }

        [HttpGet, Route("Game/{gameId}")]
        public ActionResult<Game> GetGame(int gameId)
        {
            return Ok(_gameRepository.GetGame(gameId));
        }
    }
}
