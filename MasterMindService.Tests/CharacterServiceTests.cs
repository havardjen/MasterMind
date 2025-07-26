using MasterMindResources;
using MasterMindResources.Interfaces;
using MasterMindResources.Models;
using Moq;
using Xunit;

namespace MasterMindService.Tests
{
    public class CharacterServiceTests
    {
        public CharacterServiceTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _charactersRepositoryMock = new Mock<ICharactersRepository>();
            _charactersRepositoryMock.Setup(s => s .GetCharacter(It.IsAny<string>(), true)).Returns(_validChars);
            _characterService = new CharactersService(_charactersRepositoryMock.Object, _gameRepositoryMock.Object);

            _solution = GetSolutionForGameRepository(_gameId, _solutionChars);
        }

        int _gameId = 1;
        List<string> _solutionChars = new List<string> { "A", "B", "C", "D" };
        List<string> _validChars = new List<string> { "A", "B", "C", "D", "E", "F", "" };
        Attempt _solution;

        private ICharactersService _characterService;
        private Mock<IGameRepository> _gameRepositoryMock;
        private Mock<ICharactersRepository> _charactersRepositoryMock;

        [Fact]
        public void GetHints_NoCharsInCorrPos_ResultReturned()
        {
            // Arrange
            var attempt = SetAllCharactersInWrongPosition(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "WWWW";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            _gameRepositoryMock.Verify(gr => gr.GetAttempts(_gameId), Times.Once);
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_FourEqualCharsOneInCorrPos_ResultReturned()
        {
            // Arrange
            var attempt = SetAllCharactersEqualWhichExistsInSolution(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "B";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            _gameRepositoryMock.Verify(gr => gr.GetAttempts(_gameId), Times.Once);
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_AllCharsInCorrectPosition_ResultReturned()
        {
            // Arrange
            var attempt = SetAllCharactersInCorrectPosition(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "BBBB";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_AllCharsCorrPossLowerCase_ResultReturned()
        {
            // Arrange
            var attempt = SetAllCharactersInCorrectPositionLowerCase(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "BBBB";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }


        [Fact]
        public void GetHints_ThreeCorrectOneMissing_ResultReturned()
        {
            // Arrange
            var attempt = SetThreeCorrectOneMissing(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "BBB";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_1stNotInGame2ndWrongPos3rd4thCorrect_ResultReturned()
        {
            // Arrange
            var attempt = Set1stNotInGame2ndWrongPos3rd4thCorrect(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "BBW";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_1st2ndCorrPos3rd4thMissing_ResultReturned()
        {
            // Arrange
            var attempt = Set1st2ndCorrPos3rd4thMissing(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "BB";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_1stWrong2ndCorr3rdWrong4thMissing_ResultReturned()
        {
            // Arrange
            var attempt = Set1stWrong2ndCorr3rdWrong4thMissing(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "BWW";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_1stMissing2ndCorr3rdWrongPos4thMissing_ResultReturned()
        {
            // Arrange
            var attempt = Set1stMissing2ndCorr3rdWrongPos4thMissing(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = "BW";

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }

        [Fact]
        public void GetHints_NoUsedChars_EmptyHints()
        {
            // Arrange
            var attempt = SetAllCharactersToNotInSolution(_solution);
            _gameRepositoryMock.Setup(gr => gr.GetAttempts(_gameId)).Returns(new List<Attempt> { _solution, attempt });
            string expectedHints = string.Empty;

            // Act
            string hints = _characterService.GetHints(_gameId);

            // Assert
            Assert.Equal(expectedHints, hints);
        }


        private string GetRandomCharNotInSolution(Attempt solution)
        {
            var validChars = _characterService.GetCharacter("W", true);
            var charToUse = string.Empty;
            foreach (var c in validChars)
            {
                if (!solution.ValuesList.Contains(c))
                {
                    charToUse = c;
                    break;
                }
            }

            return charToUse;
        }

        private Attempt GetSolutionForGameRepository(int gameId, List<string> solution)
        {
            return new Attempt
            {
                GameId = gameId,
                AttemptId = 1,
                AttemptType = AttemptType.Solution,
                ValueOne = solution[0],
                ValueTwo = solution[1],
                ValueThree = solution[2],
                ValueFour = solution[3]
            };
        }

        private Attempt SetAllCharactersToNotInSolution(Attempt solution)
        {
            var validChars = _characterService.GetCharacter("W", true);
            var charToUse = GetRandomCharNotInSolution(solution);

            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = charToUse,
                ValueTwo = charToUse,
                ValueThree = charToUse,
                ValueFour = charToUse
            };
        }

        private Attempt SetAllCharactersInWrongPosition(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = solution.ValueTwo,
                ValueTwo = solution.ValueOne,
                ValueThree = solution.ValueFour,
                ValueFour = solution.ValueThree
            };
        }

        private Attempt SetAllCharactersEqualWhichExistsInSolution(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = solution.ValueTwo,
                ValueTwo = solution.ValueTwo,
                ValueThree = solution.ValueTwo,
                ValueFour = solution.ValueTwo
            };
        }

        private Attempt SetAllCharactersInCorrectPosition(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = solution.ValueOne,
                ValueTwo = solution.ValueTwo,
                ValueThree = solution.ValueThree,
                ValueFour = solution.ValueFour
            };
        }

        private Attempt SetThreeCorrectOneMissing(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = solution.ValueOne,
                ValueTwo = solution.ValueTwo,
                ValueThree = string.Empty,
                ValueFour = solution.ValueFour
            };
        }

        private Attempt SetAllCharactersInCorrectPositionLowerCase(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = solution.ValueOne.ToLower(),
                ValueTwo = solution.ValueTwo.ToLower(),
                ValueThree = solution.ValueThree.ToLower(),
                ValueFour = solution.ValueFour.ToLower()
            };
        }

        private Attempt Set1stMissing2ndCorr3rdWrongPos4thMissing(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = string.Empty,
                ValueTwo = solution.ValueTwo,
                ValueThree = solution.ValueFour,
                ValueFour = string.Empty
            };
        }

        private Attempt Set1stWrong2ndCorr3rdWrong4thMissing(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = solution.ValueFour,
                ValueTwo = solution.ValueTwo,
                ValueThree = solution.ValueOne,
                ValueFour = string.Empty
            };
        }

        private Attempt Set1st2ndCorrPos3rd4thMissing(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = solution.ValueOne,
                ValueTwo = solution.ValueTwo,
                ValueThree = string.Empty,
                ValueFour = string.Empty
            };
        }

        private Attempt Set1stNotInGame2ndWrongPos3rd4thCorrect(Attempt solution)
        {
            return new Attempt
            {
                GameId = solution.GameId,
                AttemptId = solution.AttemptId + 1,
                AttemptType = AttemptType.Attempt,
                ValueOne = GetRandomCharNotInSolution(solution),
                ValueTwo = solution.ValueOne,
                ValueThree = solution.ValueThree,
                ValueFour = solution.ValueFour
            };
        }
    }
}
