using System;
using System.Collections.Generic;


namespace MasterMindResources.Interfaces
{
	public interface IGameAccess
	{
		int CreateGame();
		List<string> GetGame(int gameId);
		bool RegisterAttempt(int gameId, List<string> attempt);
		string GetHints(int gameId, List<string> attempt);
	}
}
