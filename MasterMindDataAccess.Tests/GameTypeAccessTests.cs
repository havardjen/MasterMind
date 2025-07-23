using MasterMindResources.Interfaces;
using System;
using Xunit;

namespace MasterMindDataAccess.Tests
{
    public class GameTypeAccessTests
    {
		public GameTypeAccessTests()
		{
            //var connectionString = @"D:\OneDrive\Utvikling\MasterMind";
            var connectionString = @"Data Source=C:\Users\jensaas_h\source\repos\MasterMind\MasterMindDB.db";
            _gameTypeAccessor = new GameTypeRepository(connectionString);
		}

		IGameTypeRepository _gameTypeAccessor;

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
			int gameTypeId = 14;  // Volatile... For now, we accept the risk that all types might be deleted, and new ids come into play.
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
			int expectedGameTypeId = 14;  // Volatile... For now, we accept the risk that all types might be deleted, and new ids come into play.

			// Act
			int gameTypeId = _gameTypeAccessor.GetGameTypeIdByGameType(gameType);

			// Assert
			Assert.Equal(expectedGameTypeId, gameTypeId);
		}
	}
}
