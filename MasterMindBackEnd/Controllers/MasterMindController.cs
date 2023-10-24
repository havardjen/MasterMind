using MasterMindDataAccess;
using MasterMindResources.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace MasterMindBackEnd.Controllers
{
	[RoutePrefix("api/MasterMind")]
	public class MasterMindController : ApiController
	{
		public MasterMindController(ICharactersService charService, string connectionString)
        {
			_gameTypeAccessor = new GameTypeAccess(connectionString);
			_gameAccessor = new GameAccess(connectionString);
			_charactersService = charService;
		}

		private IGameTypeAccess _gameTypeAccessor;
		private IGameAccess _gameAccessor;
		private ICharactersService _charactersService;

		[HttpGet, Route("")]
		public string Get()
		{
			return "Welcome to this super incredible amazing relatively ok MasterMind service";
		}

		[HttpGet, Route("ValidCharacters")]
		public List<string> GetValidCharacters()
		{
			return _charactersService.GetCharacter(string.Empty, true);
		}


		[HttpGet, Route("Game/Create")]
		public int CreateGame()
		{
			return _gameAccessor.CreateGame();
		}

		[HttpGet, Route("Game/{gameId}")]
		public List<string> GetGame(int gameId)
		{
			return _gameAccessor.GetGame(gameId);
		}

		[HttpPost, Route("Game/Attempt/{att}")]
		public string AttemptGame(string att)
		{
			string[] arString = att.Split('_');
			int gameId = int.Parse(arString[0]);
			List<string> attempt = new List<string> { arString[1][0].ToString(), arString[1][1].ToString(), arString[1][2].ToString(), arString[1][3].ToString() };
			_gameAccessor.RegisterAttempt(gameId, attempt);

			List<string> game = _gameAccessor.GetGame(gameId);
			return _gameAccessor.GetHints(game, attempt);
		}

		[HttpGet, Route("GameType/{id}")]
		public string GetGameType(int id)
		{
			return _gameTypeAccessor.GetGameType(id);
		}

		[HttpPost, Route("GameType/Create/{gameType}")]
		public int CreateGameType(string gameType)
		{
			return _gameTypeAccessor.CreateGameType(gameType);
		}
	}
}