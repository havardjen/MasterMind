using MasterMindResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindService
{
    public class CharactersService : ICharactersService
    {
        public CharactersService(ICharactersRepository charRepository)
        {
            _characterRepository = charRepository;
        }

        ICharactersRepository _characterRepository;

        public ICharactersRepository CharacterRepository => _characterRepository;

        public List<string> GetCharacter(string charToGet, bool getAll = false)
        {
            return _characterRepository.GetCharacter(charToGet, getAll);
        }
    }
}
