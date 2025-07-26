using MasterMindResources.Interfaces;
using System.Collections.Generic;
using Xunit;

namespace MasterMindDataAccess.Tests
{
	public class CharactersAccessTests
	{
		public CharactersAccessTests()
		{
            _chars = new CharactersRepository(@"Data Source = C:\Users\havar\OneDrive\Utvikling\MasterMind\MasterMindAPI\MasterMindDB.db");
            //_chars = new CharactersRepository(@"Data Source=C:\Users\jensaas_h\source\repos\MasterMind\MasterMindAPI\MasterMindDB.db");
        }

		ICharactersRepository _chars;
        List<string> _expectedChars = new List<string> { "A", "B", "C", "D", "E", "F", "" };

        [Fact]
		public void VerifyValidCharacters_NoCharactersExist_AllCharactersCreated()
		{
			// Arrange
			bool deleteAll = true;
			_chars.DeleteChar("w", deleteAll);

			// Act
			List<string> allChars = _chars.VerifyValidCharacters();

			// Assert
			Assert.NotNull(allChars);
			Assert.Equal(_expectedChars.Count, allChars.Count);

			for (int i = 0; i < _expectedChars.Count; i++)
				Assert.Equal(_expectedChars[i], allChars[i]);
		}

		[Fact]
		public void VerifyValidCharacters_CharacterMissing_AllCharactersPopulated()
		{
			// Arrange
			string charToDelete = "A";
			_chars.DeleteChar(charToDelete);

			// Act
			List<string> allChars = _chars.VerifyValidCharacters();

			// Assert
			Assert.NotNull(allChars);
			Assert.Equal(_expectedChars.Count, allChars.Count);

			for (int i = 0; i < _expectedChars.Count; i++)
				Assert.Equal(_expectedChars[i], allChars[i]);
		}

		[Theory]
		[InlineData("A", "B", "C", "D", true)]
		[InlineData("A", "B", "C", "G", false)]
		[InlineData("A", "_", "C", "D", false)]
		[InlineData("A", "B", "C", "2", false)]
        [InlineData("A", "B", "C", "", true)]
        [InlineData("a", "B", "C", "", true)]
        public void VerifyCharactersInGame_input_ExpectedResult(string val1, string val2, string val3, string val4, bool expectedResult)
		{
			// Arrange
			List<string> inputGame = new List<string> { val1, val2, val3, val4 };

			// Act
			bool result = _chars.VerifyCharactersInGame(inputGame);

			// Assert
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void InsertChar_ValidCharacterDoesNotExist_CharacterInserted()
		{
			// Arrange
			string charToInsert = "A";
			_chars.VerifyValidCharacters(); // To ensure all characters are there.
			_chars.DeleteChar(charToInsert);
			
			// Act
			_chars.InsertChar(charToInsert);
			string charFromDb = _chars.GetCharacter(charToInsert)[0];

			// Assert
			Assert.NotNull(charFromDb);
			Assert.Equal(charToInsert, charFromDb);
		}

		[Fact]
		public void GetCharacter_GetAll_AllCharactersReturned()
		{
			// Arrange
			bool getAll = true;
			
			_chars.VerifyValidCharacters();

			// Act
			List<string> allChars = _chars.GetCharacter("w", getAll);

			// Assert
			Assert.NotNull(allChars);
			Assert.Equal(_expectedChars.Count, allChars.Count);

			for (int i = 0; i < _expectedChars.Count; i++)
				Assert.Equal(_expectedChars[i], allChars[i]);
		}

		[Fact]
		public void DeleteChar_deleteAll_CharactersDeleted()
		{
			// Arrange
			bool deleteAll = true;
			bool getAll = true;
			_chars.VerifyValidCharacters(); // To ensure that the table is populated with characters before the test is run.

			// Act
			_chars.DeleteChar("w", deleteAll);
			var chars = _chars.GetCharacter("w", getAll);

			// Assert
			Assert.Single(chars);
			Assert.Equal("", chars[0]);
		}

		[Fact]
		public void DeleteChar_deleteSingleChar_CharacterDeleted()
		{
			// Arrange
			bool getAll = true;
			string charToDelete = "A";
			_chars.VerifyValidCharacters(); // To ensure that the table is populated with characters before the test is run.
			List<string> charsBefore = _chars.GetCharacter("w", getAll);

			// Act
			_chars.DeleteChar(charToDelete);
			int numChars = _chars.GetCharacter("w", getAll).Count;

			// Assert
			Assert.Equal(charsBefore.Count - 1, numChars);
		}
	}
}
