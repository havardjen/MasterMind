using System;
using System.Collections.Generic;

namespace MasterMindResources.Interfaces
{
	public interface ICharactersService
	{
		List<string> GetCharacter(string charToGet, bool getAll = false);
	}
}
