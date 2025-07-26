using MasterMindResources;
using MasterMindResources.Interfaces;
using MasterMindResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindService
{
    public class CharactersService : ICharactersService
    {
        public CharactersService(ICharactersRepository charRepository, IGameRepository gameRepository)
        {
            _characterRepository = charRepository;
            _gameRepository = gameRepository;

            _validCharacters = _characterRepository.GetCharacter(string.Empty, true);
        }

        private ICharactersRepository _characterRepository;
        private IGameRepository _gameRepository;
        private List<string> _validCharacters;

        private const string CHAR_IN_CORRECT_POSITION = "B";
        private const string CHAR_IN_WRONG_POSITION = "W";

        public ICharactersRepository CharacterRepository => _characterRepository;

        private bool IsValidChar(string charToTest)
        {
            bool isValid = false;

            if (_validCharacters.Contains(charToTest))
                isValid = true;

            return isValid;
        }

        public List<string> GetCharacter(string charToGet, bool getAll = false)
        {
            return _characterRepository.GetCharacter(charToGet, getAll);
        }

        /// <summary>
		/// Fetches hints for the current attempt.
		/// Hints are sorted alphabetically, so that the cannot be linked to a particular position.
		/// </summary>
		/// <param name="gameId"></param>
		/// <param name="attempt"></param>
		/// <returns></returns>
        public string GetHints(int gameId)
        {
            var allAttempts = _gameRepository.GetAttempts(gameId);
            var solution = allAttempts.Where(a => a.AttemptType == AttemptType.Solution).Last();

            Attempt attempt;
            try
            {
                attempt = allAttempts.Where(a => a.AttemptType == AttemptType.Attempt).Last();
            }
            catch (InvalidOperationException)
            {
                // No attempts made yet, return empty hints
                return string.Empty;
            }


            string result = string.Empty;
            var validCharsCount = new Dictionary<string, int> { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 }, { "E", 0 }, { "F", 0 }, { "", 0 } };

            attempt.ValueOne = attempt.ValueOne.ToUpper();
            attempt.ValueTwo = attempt.ValueTwo.ToUpper();
            attempt.ValueThree = attempt.ValueThree.ToUpper();
            attempt.ValueFour = attempt.ValueFour.ToUpper();


            List<string> tmpHints = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                if (attempt.ValuesList[i] == solution.ValuesList[i])
                {
                    tmpHints.Add(CHAR_IN_CORRECT_POSITION);
                    validCharsCount[attempt.ValuesList[i]]++;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                string attChar = attempt.ValuesList[i];
                if ((attChar != solution.ValuesList[i]))
                {
                    int count = solution.ValuesList.Where(x => x == attChar).Count();

                    if (IsValidChar(attChar) && validCharsCount[attChar] < count)
                    {
                        validCharsCount[attChar]++;
                        tmpHints.Add(CHAR_IN_WRONG_POSITION);
                    }
                }

            }

            tmpHints.Sort();
            foreach (string h in tmpHints)
                result += h;

            return result;
        }
    }
}
