using System;

namespace MasterMindResources.Interfaces
{
	public interface IGameTypeAccess
	{
		int CreateGameType(string gameType);
		int GetGameTypeIdByGameType(string gameType);
		string GetGameType(int gameTypeId);
	}
}
