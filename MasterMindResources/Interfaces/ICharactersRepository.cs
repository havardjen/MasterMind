using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindResources.Interfaces
{
    public interface ICharactersRepository
    {
        List<string> VerifyValidCharacters();
        bool VerifyCharactersInGame(List<string> game);
        bool DeleteChar(string charToDelete, bool deleteAll = false);
        List<string> GetCharacter(string charToGet, bool getAll = false);
        void InsertChar(string charToInsert);
    }
}
