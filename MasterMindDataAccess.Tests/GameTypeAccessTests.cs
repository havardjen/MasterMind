using MasterMindResources.Interfaces;
using System;
using Xunit;

namespace MasterMindDataAccess.Tests
{
    public class GameTypeAccessTests
    {
		public GameTypeAccessTests()
		{
			_gameTypeAccessor = new GameTypeAccess();
		}

		IGameTypeAccess _gameTypeAccessor;

		[Fact]
		public void CreateGameType_GameTypeDoesNotExist_GameTypeIdReturned()
		{
			// Arrange
			string gameType = "TEST_" + DateTime.Now.ToString();

			// Act
			int gameTypeId = _gameTypeAccessor.CreateGameType(gameType);

			// Assert
			Assert.True(gameTypeId > 0);
		}

		[Fact]
		public void GetGameType_GameTypeExists_GameTypeReturned()
		{
			// Arrange
			int gameTypeId = 2;
			string expectedGameType = "Game";

			// Act
			string gameType = _gameTypeAccessor.GetGameType(gameTypeId);

			// Assert
			Assert.Equal(expectedGameType, gameType);
		}

		[Fact]
		public void GetGameTypeIdByGameType_GameTypeExists_GameTypeIdReturned()
		{
			// Arrange
			string gameType = "Game";
			int expectedGameTypeId = 2;

			// Act
			int gameTypeId = _gameTypeAccessor.GetGameTypeIdByGameType(gameType);

			// Assert
			Assert.Equal(expectedGameTypeId, gameTypeId);
		}
	}
}
