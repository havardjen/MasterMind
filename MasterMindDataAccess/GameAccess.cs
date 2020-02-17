using MasterMindResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;


namespace MasterMindDataAccess
{
	public class GameAccess : IGameAccess
	{
		public GameAccess()
		{
			_connectionString = @"Data Source=C:\Users\havar\source\repos\MasterMind\MasterMindDB.db";
			_gameTypeAccessor = new GameTypeAccess();
			_charAccessor = new CharactersAccess();

			_validCharacters = _charAccessor.GetCharacter(string.Empty, true);
			VerifyGameTypes();
		}

		string _connectionString;
		IGameTypeAccess _gameTypeAccessor;
		ICharactersAccess _charAccessor;
		List<string> _validCharacters;

		private const string GAME = "Game";
		private const string ATTEMPT = "Attempt";
		private const string CHAR_IN_CORRECT_POSITION = "B";
		private const string CHAR_IN_WRONG_POSITION = "W";

		/// <summary>
		/// Game and Attempt are the two essential game types.
		/// Here we assure that they always exist.
		/// </summary>
		private void VerifyGameTypes()
		{
			int gameTypeId = _gameTypeAccessor.GetGameTypeIdByGameType(GAME);

			if (!(gameTypeId > 0))
				_gameTypeAccessor.CreateGameType(GAME);

			gameTypeId = _gameTypeAccessor.GetGameTypeIdByGameType(ATTEMPT);

			if (!(gameTypeId > 0))
				_gameTypeAccessor.CreateGameType(ATTEMPT);
		}

		private bool IsValidChar(string charToTest)
		{
			bool isValid = false;

			if (_validCharacters.Contains(charToTest))
				isValid = true;

			return isValid;
		}

		public int CreateGame()
		{
			int gameId = 0;
			int gametypeId = _gameTypeAccessor.GetGameTypeIdByGameType(GAME);

			List<string> allCharsInDb = _charAccessor.GetCharacter("w", true);
			List<string> newGame = allCharsInDb.OrderBy(x => Guid.NewGuid()).Take(4).ToList();  // Sorting the list in a random order using Guid.
																								// Then the first four items are picked.

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = $@"INSERT INTO Game(GameTypeId, Value1, Value2, Value3, Value4)
									  VALUES('{gametypeId}', '{newGame[0]}', '{newGame[1]}', '{newGame[2]}', '{newGame[3]}');";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

				conn.Open();
				
				cmd.ExecuteScalar();

				queryText = "SELECT last_insert_rowid();";
				cmd = new SQLiteCommand(queryText, conn);
				gameId = Convert.ToInt32(cmd.ExecuteScalar());
				conn.Close();
			}

			return gameId;
		}

		public List<string> GetGame(int gameId)
		{
			List<string> game = new List<string>();
			string tmpGame = string.Empty;

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = @$"SELECT g.Value1 || g.Value2 || g.Value3 || g.Value4 as GameFetched
									 FROM Game g
									 WHERE g.GameId = {gameId};";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);
				conn.Open();
				tmpGame = cmd.ExecuteScalar().ToString();
				conn.Close();
			}

			game.Add(tmpGame.ElementAt(0).ToString());
			game.Add(tmpGame.ElementAt(1).ToString());
			game.Add(tmpGame.ElementAt(2).ToString());
			game.Add(tmpGame.ElementAt(3).ToString());

			return game;
		}

		public bool RegisterAttempt(int gameId, List<string> attempt)
		{
			bool result = false;
			int attemptTypeId = _gameTypeAccessor.GetGameTypeIdByGameType(ATTEMPT);

			if (_charAccessor.VerifyCharactersInGame(attempt))
			{
				using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
				{
					string queryText = $@"INSERT INTO Game(GameTypeId, Value1, Value2, Value3, Value4)
									  VALUES('{attemptTypeId}', '{attempt[0]}', '{attempt[1]}', '{attempt[2]}', '{attempt[3]}');";

					SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

					conn.Open();

					cmd.ExecuteScalar();

					queryText = "SELECT last_insert_rowid();";
					cmd = new SQLiteCommand(queryText, conn);
					int attemptId = Convert.ToInt32(cmd.ExecuteScalar());


					queryText = $@"INSERT INTO Attempt(AttemptId, GameId)
							   VALUES({attemptId}, {gameId});";
					cmd = new SQLiteCommand(queryText, conn);
					cmd.ExecuteNonQuery();

					conn.Close();
				}

				result = true;
			}
			else
				result = false;

			return result;
		}

		/// <summary>
		/// Fetches hints for the current attempt.
		/// Hints are sorted alphabetically, so that the cannot be linked to a particular position.
		/// </summary>
		/// <param name="gameId"></param>
		/// <param name="attempt"></param>
		/// <returns></returns>
		public string GetHints(List<string> game, List<string> attempt)
		{
			string result = string.Empty;
			Dictionary<string, int> ValidCharsCount = new Dictionary<string, int> { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 }, { "E", 0 }, { "F", 0 } };

			for (int i = 0; i < attempt.Count; i++)
				attempt[i] = attempt[i].ToUpper();

			List<string> tmpHints = new List<string>();
			for(int i=0; i<4; i++)
			{
				if (attempt[i] == game[i])
				{
					tmpHints.Add(CHAR_IN_CORRECT_POSITION);
					ValidCharsCount[attempt[i]]++;
				}
					
			}

			for (int i = 0; i < 4; i++)
			{
				string attChar = attempt[i];
				if ((attChar != game[i]))
				{
					int count = game.Where(x => x == attChar).Count();

					if(IsValidChar(attChar) && ValidCharsCount[attChar] < count)
					{
						ValidCharsCount[attChar]++;
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
