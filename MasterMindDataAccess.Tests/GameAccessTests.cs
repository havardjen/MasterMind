using MasterMindResources.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace MasterMindDataAccess.Tests
{
	public class GameAccessTests
	{
		public GameAccessTests()
		{
			_gameAccessor = new GameAccess();
		}

		IGameAccess _gameAccessor;

		[Fact]
		public void CreateGame_ValidCharacters_GameCreatedGameIdReturned()
		{
			// Arrange

			// Act
			int gameId = _gameAccessor.CreateGame();

			// Assert
			Assert.True(gameId > 0);
		}

		[Fact]
		public void GetGame_GameIdExists_GameReturned()
		{
			// Arrange
			int gameId = 3;  // shortcut, for now we'll have to trust this gameId exists in the database.

			// Act
			List<string> Game = _gameAccessor.GetGame(gameId);

			// Assert
			Assert.NotNull(Game);
			Assert.Equal(4, Game.Count);
		}

		[Fact]
		public void RegisterAttempt_ValidCharacters_AttemptRegisteredTrueReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> attempt = new List<string> { "A", "B", "C", "D" };

			// Act
			bool attemptRegistered = _gameAccessor.RegisterAttempt(gameId, attempt);

			// Assert
			Assert.True(attemptRegistered);
		}

		[Fact]
		public void RegisterAttempt_FromGuiAttempt_AttemptRegisteredTrueReturned()
		{
			// Arrange
			int gameId = 41;
			List<string> attempt = new List<string> { "A", "B", "C", "D" };

			// Act
			bool attemptRegistered = _gameAccessor.RegisterAttempt(gameId, attempt);

			// Assert
			Assert.True(attemptRegistered);
		}

		[Fact]
		public void GetHints_NoCharsInCorrPos_ResultReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);
			
			List<string> attempt = new List<string>();
			attempt.Add(game[3]);
			attempt.Add(game[2]);
			attempt.Add(game[1]);
			attempt.Add(game[0]);

			string expectedHints = "WWWW";

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_AllCharsInCorrectPosition_ResultReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);

			List<string> attempt = new List<string>();
			attempt.Add(game[0]);
			attempt.Add(game[1]);
			attempt.Add(game[2]);
			attempt.Add(game[3]);

			string expectedHints = "BBBB";

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_AllCharsCorrPossLowerCase_ResultReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);

			List<string> attempt = new List<string>();
			attempt.Add(game[0].ToLower());
			attempt.Add(game[1].ToLower());
			attempt.Add(game[2].ToLower());
			attempt.Add(game[3].ToLower());

			string expectedHints = "BBBB";

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_ThreeCorrectOneMissing_ResultReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);

			List<string> attempt = new List<string>();
			attempt.Add(game[0]);
			attempt.Add(game[1]);
			attempt.Add(game[2]);
			attempt.Add("H");		// We use an invalid characters, to make sure that the 4th char in attempt is not a correct one in correct position.

			string expectedHints = "BBB";

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_2ndWrongPos3rd4thCorrect_ResultReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);

			List<string> attempt = new List<string>();
			attempt.Add("H");		// We use an invalid characters, to make sure that the 4th char in attempt is not a correct one in correct position.
			attempt.Add(game[0]);
			attempt.Add(game[2]);
			attempt.Add(game[3]);       

			string expectedHints = "BBW";

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_1st2ndCorrPos3rd4thMissing_ResultReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);

			List<string> attempt = new List<string>();
			attempt.Add(game[0]);
			attempt.Add(game[1]);
			attempt.Add("O");       // We use an invalid characters, to make sure that the 4th char in attempt is not a correct one in correct position.
			attempt.Add("P");

			string expectedHints = "BB";

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_1stWrong2ndCorr3rdWrong4thMissing_ResultReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);

			List<string> attempt = new List<string>();
			attempt.Add("H");  // We use an invalid characters, to make sure that the 4th char in attempt is not a correct one in correct position.
			attempt.Add(game[1]);
			attempt.Add(game[0]);       
			attempt.Add(game[2]);

			string expectedHints = "BWW";

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}

		[Fact]
		public void GetHints_NoCorrectCharsReturned()
		{
			// Arrange
			int gameId = _gameAccessor.CreateGame();
			List<string> game = _gameAccessor.GetGame(gameId);

			List<string> attempt = new List<string>();
			attempt.Add("H");  // We use an invalid characters, to make sure that the 4th char in attempt is not a correct one in correct position.
			attempt.Add("H");
			attempt.Add("H");
			attempt.Add("H");

			string expectedHints = string.Empty;

			// Act
			string hints = _gameAccessor.GetHints(gameId, attempt);

			// Assert
			Assert.Equal(expectedHints, hints);
		}
	}
}
