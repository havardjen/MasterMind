using System;

namespace MasterMindResources.Interfaces
{
	public interface IGameTypeRepository
	{
		int CreateGameType(string gameType);
		int GetGameTypeIdByGameType(string gameType);
		string GetGameType(int gameTypeId);
	}
}
