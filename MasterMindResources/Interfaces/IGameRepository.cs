using MasterMindResources.Models;
using System.Collections.Generic;


namespace MasterMindResources.Interfaces
{
	public interface IGameRepository
	{
		int CreateGame();

		Game GetGame(int gameId);
		
		Attempt GetSolution(int gameId);

        int RegisterAttempt(int gameId, List<string> attempt, AttemptType attemptType = AttemptType.Attempt);

        Dictionary<int, AttemptType> GetAttemptTypes();
        
		List<Attempt> GetAttempts(int gameId);

		Attempt GetAttempt(int attemptId);

        bool SaveAttempt(Attempt attempt);

        AttemptType GetAttemptType(int attemptId);
    }
}
