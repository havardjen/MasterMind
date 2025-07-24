using MasterMindResources.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace MasterMindDataAccess.Tests
{
	public class GameReposityTests
	{
		public GameReposityTests()
		{
            //string connectionString = @"D:\OneDrive\Utvikling\MasterMind";
            string connectionString = @"Data Source=C:\Users\jensaas_h\source\repos\MasterMind\MasterMindDB.db";

			_characterRepository = new CharactersRepository(connectionString);
            _gameRepository = new GameRepository(connectionString, _characterRepository);
		}

		IGameRepository _gameRepository;
		ICharactersRepository _characterRepository;

        [Fact]
		public void CreateGame_ValidCharacters_GameCreatedGameIdReturned()
		{
            // Arrange

            //TODO: Ensure that the first line in Attemps is marked as the solution. Preferably this line has a certain typeId(e.g. 1) to indicate it is the solution.

            // Act
            int gameId = _gameRepository.CreateGame();

			// Assert
			Assert.True(false);
		}

		[Fact]
		public void GetGame_GameIdExists_GameReturned()
		{
			// Arrange
			int gameId = 163;  // shortcut, for now we'll have to trust this gameId exists in the database.

			// Act
			var Game = _gameRepository.GetGame(gameId);

			// Assert
			Assert.NotNull(Game);
			Assert.Equal(gameId, Game.GameId);
			Assert.True(Game.CreatedDate > DateTime.MinValue);
			Assert.True(Game.ModifiedDate > DateTime.MinValue);
		}

		[Fact]
		public void RegisterAttempt_ValidCharacters_AttemptRegisteredTrueReturned()
		{
			// Arrange
			int gameId = _gameRepository.CreateGame();
			List<string> attempt = new List<string> { "A", "B", "C", "D" };

			// Act
			bool attemptRegistered = _gameRepository.RegisterAttempt(gameId, attempt);

			// Assert
			Assert.True(false);  // This is a placeholder assertion. Ensure that the attempt is registered successfully, with correct attemptId. There must be only one attempt with attemptId = 1, which is the solution.
        }


		[Fact]
		public void GetHints_NoCharsInCorrPos_ResultReturned()
		{
			// Arrange
			//int gameId = _gameRepository.CreateGame();
			//List<string> game = _gameRepository.GetGame(gameId);
			
			//List<string> attempt = new List<string>();
			//attempt.Add(game[3]);
			//attempt.Add(game[2]);
			//attempt.Add(game[1]);
			//attempt.Add(game[0]);

			string expectedHints = "WWWW";

			// Act
			//string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal("A", "B");  // This is a placeholder assertion, replace with actual hints retrieval logic.
        }

		[Fact]
		public void GetHints_FourEqualCharsOneInCorrPos_ResultReturned()
		{
            // Arrange

            //TODO: GetHints() needs to check against the first line in Attempts, which is the solution. Preferably this line has a certain typeId(e.g. 1) to indicate it is the solution.

            List<string> game = new List<string> { "A", "B", "C", "D" };
			List<string> attempt = new List<string> { "A", "A", "A", "A" };

			string expectedHints = "B";

			// Act
			string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal("A", "B");
		}

		[Fact]
		public void GetHints_AllCharsInCorrectPosition_ResultReturned()
		{
			// Arrange
			int gameId = _gameRepository.CreateGame();
			//List<string> game = _gameRepository.GetGame(gameId);

			//List<string> attempt = new List<string>();
			//attempt.Add(game[0]);
			//attempt.Add(game[1]);
			//attempt.Add(game[2]);
			//attempt.Add(game[3]);

			string expectedHints = "BBBB";

			// Act
			//string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal("A", "B"); // This is a placeholder assertion, replace with actual hints retrieval logic.
        }

		[Fact]
		public void GetHints_AllCharsCorrPossLowerCase_ResultReturned()
		{
			// Arrange
			//int gameId = _gameRepository.CreateGame();
			//List<string> game = _gameRepository.GetGame(gameId);

			//List<string> attempt = new List<string>();
			//attempt.Add(game[0].ToLower());
			//attempt.Add(game[1].ToLower());
			//attempt.Add(game[2].ToLower());
			//attempt.Add(game[3].ToLower());

			string expectedHints = "BBBB";

			// Act
			//string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal("A", "B"); // This is a placeholder assertion, replace with actual hints retrieval logic.
        }

		[Fact]
		public void GetHints_ThreeCorrectOneMissing_ResultReturned()
		{
			// Arrange
			List<string> game = new List<string> { "A", "B", "C", "D" };
			List<string> attempt = new List<string> { "A", "B", "C", "E" };

			string expectedHints = "BBB";

			// Act
			string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_2ndWrongPos3rd4thCorrect_ResultReturned()
		{
			// Arrange
			List<string> game = new List<string> { "A", "B", "C", "D" };
			List<string> attempt = new List<string> { "F", "A", "C", "D" };

			string expectedHints = "BBW";

			// Act
			string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_1st2ndCorrPos3rd4thMissing_ResultReturned()
		{
			// Arrange
			List<string> game = new List<string> { "A", "B", "C", "D" };
			List<string> attempt = new List<string> { "A", "B", "F", "F" };

			string expectedHints = "BB";

			// Act
			string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_1stWrongPos2ndCorr3rdWrongPos4thMissing_ResultReturned()
		{
			// Arrange
			List<string> game = new List<string> { "A", "B", "C", "D" };
			List<string> attempt = new List<string> { "D", "B", "F", "C" };

			string expectedHints = "BWW";

			// Act
			string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_1stMissing2ndCorr3rdWrongPos4thMissing_ResultReturned()
		{
			// Arrange
			List<string> game = new List<string> { "A", "B", "C", "D" };
			List<string> attempt = new List<string> { "F", "B", "D", "E" };

			string expectedHints = "BW";

			// Act
			string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_NoCorrectCharsReturned()
		{
			// Arrange
			List<string> game = new List<string> { "A", "B", "A", "A" };
			List<string> attempt = new List<string> { "C", "D", "E", "F" };

			string expectedHints = string.Empty;

			// Act
			string hints = _gameRepository.GetHints(game, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}
	}
}
