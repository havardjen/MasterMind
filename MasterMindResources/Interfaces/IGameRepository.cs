using MasterMindResources.Models;
using System.Collections.Generic;


namespace MasterMindResources.Interfaces
{
	public interface IGameRepository
	{
		int CreateGame();

		Game GetGame(int gameId);

		bool RegisterAttempt(int gameId, List<string> attempt);

		string GetHints(List<string> game, List<string> attempt);
	}
}
