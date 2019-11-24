using System;
using System.Collections.Generic;

namespace MasterMindResources.Interfaces
{
	public interface ICharactersAccess
	{
		List<string> VerifyValidCharacters();
		bool VerifyCharactersInGame(List<string> game);
		bool DeleteChar(string charToDelete, bool deleteAll = false);
		List<string> GetCharacter(string charToGet, bool getAll = false);
		void InsertChar(string charToInsert);
	}
}
