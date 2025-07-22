using System;
using System.Collections.Generic;


namespace MasterMindResources.Interfaces
{
	public interface IGameRepository
	{
		int CreateGame();
		List<string> GetGame(int gameId);
		bool RegisterAttempt(int gameId, List<string> attempt);
		string GetHints(List<string> game, List<string> attempt);
	}
}
