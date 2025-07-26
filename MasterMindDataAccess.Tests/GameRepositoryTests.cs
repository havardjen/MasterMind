using MasterMindResources;
using MasterMindResources.Interfaces;
using MasterMindResources.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Xunit;

namespace MasterMindDataAccess.Tests
{
	public class GameReposityTests
	{
		public GameReposityTests()
		{
            _connectionString = @"Data Source = C:\Users\havar\OneDrive\Utvikling\MasterMind\MasterMindAPI\MasterMindDB.db";
            //_connectionString = @"Data Source=C:\Users\jensaas_h\source\repos\MasterMind\MastermindAPI\MasterMindDB.db";

            _characterRepository = new CharactersRepository(_connectionString);
            _gameRepository = new GameRepository(_connectionString, _characterRepository);

			_gameId = 163; // This is a hardcoded _gameId for testing purposes, assuming it exists in the database.
        }

		IGameRepository _gameRepository;
		ICharactersRepository _characterRepository;
		string _connectionString;
		int _gameId;

        [Fact]
        public void GetAttemptTypes_NoInput_BothAttemptTypesExist()
        {
            // Arrange

            // Act
            var attemptTypes = _gameRepository.GetAttemptTypes();

            // Assert
			Assert.NotNull(attemptTypes);
            Assert.Equal(2, attemptTypes.Count);

			Assert.True(attemptTypes.ElementAt(0).Key > 0);
            Assert.Equal(AttemptType.Solution, attemptTypes.ElementAt(0).Value);
            Assert.True(attemptTypes.ElementAt(1).Key > 0);
			Assert.Equal(AttemptType.Attempt, attemptTypes.ElementAt(1).Value);
        }

		[Fact]
		public void GetSolution_GameIdExists_SolutionReturned()
		{
			// Arrange
			
            // Act
			var solution = _gameRepository.GetSolution(_gameId);

            // Assert
			Assert.NotNull(solution);
			Assert.Equal(AttemptType.Solution, solution.AttemptType);
			AssertAllFourValuesAreSet(solution);
            Assert.True(string.IsNullOrEmpty(solution.Hints));
        }

        [Fact]
		public void CreateGame_ValidCharacters_GameCreatedGameIdReturned()
		{
            // Arrange

            // Act
            int gameId = _gameRepository.CreateGame();

            // Assert
            Assert.True(gameId > 0); 
            var attempt = AssertOnlyOneRowInAttemptForNewlyCreatedGame(gameId, _connectionString);
			AssertAllFourValuesAreSetAndAreUpperCase(attempt);
        }

        [Fact]
		public void GetGame_GameIdExists_GameReturned()
		{
			// Arrange

			// Act
			var Game = _gameRepository.GetGame(_gameId);

			// Assert
			Assert.NotNull(Game);
			Assert.Equal(_gameId, Game.GameId);
			Assert.True(Game.CreatedDate > DateTime.MinValue);
			Assert.True(Game.ModifiedDate > DateTime.MinValue);
		}

        [Fact]
        public void RegisterAttempt_LeastOneLowerCase_AttemptRegisteredTrueReturned()
        {
            // Arrange
            var allCharsInDb = _characterRepository.GetCharacter("w", true);
            var attempt = allCharsInDb.OrderBy(x => Guid.NewGuid()).Take(4).ToList();  // Sorting the list in a random order using Guid.
                                                                                       // Then the first four items are picked.
            attempt[2] = "a";

            // Act
            var attemptId = _gameRepository.RegisterAttempt(_gameId, attempt);

            // Assert
            Assert.True(attemptId > 0);
            AssertAttemptIsOfCorrectType(attemptId, AttemptType.Attempt);

            var attemptFromDb = _gameRepository.GetAttempt(attemptId);  
            AssertAllFourValuesAreSetAndAreUpperCase(attemptFromDb);
        }

        [Fact]
		public void RegisterAttempt_ValidCharacters_AttemptRegisteredTrueReturned()
		{
			// Arrange
            var allCharsInDb = _characterRepository.GetCharacter("w", true);
            var attempt = allCharsInDb.OrderBy(x => Guid.NewGuid()).Take(4).ToList();  // Sorting the list in a random order using Guid.
                                                                                       // Then the first four items are picked.
			
			// Act
            var attemptId = _gameRepository.RegisterAttempt(_gameId, attempt);

			// Assert
			Assert.True(attemptId > 0);
			AssertAttemptIsOfCorrectType(attemptId, AttemptType.Attempt);
        }

		[Theory]
		[InlineData(195, AttemptType.Solution)]
		[InlineData(202, AttemptType.Attempt)]
        public void GetAttemptType_ValidInput_AttemptTypeReturned(int attemptId, AttemptType expectedType)
		{
            // Arrange

            // Act
			var attemptType = _gameRepository.GetAttemptType(attemptId);

            // Assert
			Assert.Equal(expectedType, attemptType);
        }

        [Fact]
        public void GetAttempt_ValidInput_AttemptIsFetched()
        {
            // Arrange
			var expAttempt = GetLastAttempt(_gameId);

            // Act
            var attempt = _gameRepository.GetAttempt(expAttempt.AttemptId);

            // Assert
			Assert.NotNull(attempt);
			AssertAttemptHasExpectedValues(attempt, expAttempt);
        }

        [Fact]
        public void GetAttempt_AllLowerCaseInDb_AttemptIsFetchedAllUpper()
        {
            // Arrange
            var expAttempt = SetValuesForLastAttemptToLowerCase(_gameId);

            // Act
            var attempt = _gameRepository.GetAttempt(expAttempt.AttemptId);

            // Assert
            Assert.NotNull(attempt);
            AssertAttemptHasExpectedValues(attempt, expAttempt);
        }

        [Fact]
        public void SaveAttempt_ValidInput_AttemptSaved()
        {
            // Arrange
            var attempt = SetValuesForLastAttemptToAllSameCharacter(_gameId, "C");

            // Act
            var result = _gameRepository.SaveAttempt(attempt);

            // Assert
            Assert.True(result);
            var attemptInDb = _gameRepository.GetAttempt(attempt.AttemptId);
            Assert.NotNull(attemptInDb);
			AssertAttemptHasExpectedValues(attemptInDb, attempt);
        }

        [Fact]
        public void SaveAttempt_AllLowerCase_AttemptSavedAllUpper()
        {
            // Arrange
            var attempt = SetValuesForLastAttemptToLowerCase(_gameId);

            // Act
            _gameRepository.SaveAttempt(attempt);

            // Assert
            var attemptInDb = _gameRepository.GetAttempt(attempt.AttemptId);
            Assert.NotNull(attemptInDb);
            AssertAttemptHasExpectedValues(attemptInDb, attempt);
        }


        private Attempt GetLastAttempt(int gameId)
        {
            var lastAttempt = _gameRepository.GetAttempts(gameId).Last();
            return lastAttempt;
        }

        private Attempt SetValuesForLastAttemptToAllSameCharacter(int gameId, string character = "A")
        {
            var lastAttempt = GetLastAttempt(gameId);

            lastAttempt.ValueOne	= character;
            lastAttempt.ValueTwo	= character;
            lastAttempt.ValueThree	= character;
            lastAttempt.ValueFour	= character;

            return lastAttempt;
        }

        private Attempt SetValuesForLastAttemptToLowerCase(int gameId)
		{
            var lastAttempt = GetLastAttempt(gameId);

            lastAttempt.ValueOne	= lastAttempt.ValueOne.ToLower();
            lastAttempt.ValueTwo	= lastAttempt.ValueTwo.ToLower();
            lastAttempt.ValueThree	= lastAttempt.ValueThree.ToLower();
            lastAttempt.ValueFour	= lastAttempt.ValueFour.ToLower();

			SaveAttempt(lastAttempt);

            return lastAttempt;
        }

        private void SaveAttempt(Attempt attempt)
        {
            if (_characterRepository.VerifyCharactersInGame(attempt.ValuesList))
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    var queryText = $@"UPDATE Attempt
									   SET ValueOne   = '{attempt.ValueOne}', 
										   ValueTwo   = '{attempt.ValueTwo}', 
										   ValueThree = '{attempt.ValueThree}',
										   ValueFour  = '{attempt.ValueFour}'
									   WHERE AttemptId = {attempt.AttemptId}";

                    var cmd = new SQLiteCommand(queryText, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }


        private void AssertAllFourValuesAreSetAndAreUpperCase(Attempt attempt)
        {
            AssertAllFourValuesAreSet(attempt);
            Assert.Equal(attempt.ValueOne.ToUpper(), attempt.ValueOne);
            Assert.Equal(attempt.ValueTwo.ToUpper(), attempt.ValueTwo);
            Assert.Equal(attempt.ValueThree.ToUpper(), attempt.ValueThree);
            Assert.Equal(attempt.ValueFour.ToUpper(), attempt.ValueFour);
        }

        private void AssertAllFourValuesAreSet(Attempt attempt)
		{
            Assert.True(attempt.ValueOne != null);
            Assert.True(attempt.ValueTwo != null);
            Assert.True(attempt.ValueThree != null);
            Assert.True(attempt.ValueFour != null);
        }

		private Attempt AssertOnlyOneRowInAttemptForNewlyCreatedGame(int gameId, string connectionString)
		{
            var attempts = new List<Attempt>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                // Et nyopprettet spill skal ha kun èn rad, og det skal være en løsning.
                string queryText = @$"SELECT a.ValueOne, a.ValueTwo, a.ValueThree, a.ValueFour, a.AttemptId, att.AttemptType
									 FROM Attempt a
									 JOIN AttemptType att ON att.AttemptTypeId = a.AttemptTypeId
									 WHERE a.GameId = {gameId};";

                SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
						var attempt = new Attempt();
                        
						attempt.GameId = gameId;
                        attempt.AttemptId = Convert.ToInt32(reader["AttemptId"]);
                        attempt.ValueOne = reader["ValueOne"].ToString();
                        attempt.ValueTwo = reader["ValueTwo"].ToString();
                        attempt.ValueThree = reader["ValueThree"].ToString();
                        attempt.ValueFour = reader["ValueFour"].ToString();
                        attempt.AttemptType = Enum.Parse<AttemptType>(reader["AttemptType"].ToString());

						attempts.Add(attempt);	
                    }
                }
                conn.Close();
            }

            Assert.Single(attempts);
            Assert.Equal(AttemptType.Solution, attempts[0].AttemptType);

			return attempts[0];
        }

        private void AssertAttemptIsOfCorrectType(int attemptId, AttemptType expectedType)
        {
			var attemptType = _gameRepository.GetAttemptType(attemptId);
			Assert.Equal(expectedType, attemptType);
        }

		private void AssertAttemptHasExpectedValues(Attempt attempt, Attempt expecteAttempt)
		{
			Assert.Equal(expecteAttempt.ValueOne.ToUpper(), attempt.ValueOne);
			Assert.Equal(expecteAttempt.ValueTwo.ToUpper(), attempt.ValueTwo);
			Assert.Equal(expecteAttempt.ValueThree.ToUpper(), attempt.ValueThree);
			Assert.Equal(expecteAttempt.ValueFour.ToUpper(), attempt.ValueFour);
			Assert.Equal(expecteAttempt.AttemptType, attempt.AttemptType);
			Assert.Equal(expecteAttempt.GameId, attempt.GameId);
			Assert.Equal(expecteAttempt.AttemptId, attempt.AttemptId);
			Assert.Equal(expecteAttempt.Hints, attempt.Hints);
        }
    }
}
