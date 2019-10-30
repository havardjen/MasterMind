using MasterMindDataAccess;
using MasterMindResources.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace MasterMindBackEnd.Controllers
{

	[RoutePrefix("api/MasterMind")]
	public class MasterMindController : ApiController
	{
		public MasterMindController()
		{
			_gameTypeAccessor = new GameTypeAccess();
		}
		private IGameTypeAccess _gameTypeAccessor;

		[HttpGet, Route("")]
		public string Get()
		{
			return "Testing 1 2 3";
		}

		[HttpGet, Route("ValidCharacters")]
		public List<string> GetValidCharacters()
		{
			return new List<string>() { "A", "B", "C", "D", "E", "F" };
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