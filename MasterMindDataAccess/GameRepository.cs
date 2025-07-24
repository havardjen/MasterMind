using MasterMindResources.Interfaces;
using MasterMindResources.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;


namespace MasterMindDataAccess
{
	public class GameRepository : IGameRepository
	{
		public GameRepository(string connectionString, ICharactersRepository charRepository)
		{
            _connectionString = connectionString;
			_charRepository = charRepository;

			_validCharacters = _charRepository.GetCharacter(string.Empty, true);
		}

		string _connectionString;
		ICharactersRepository _charRepository;
		List<string> _validCharacters;

		private const string GAME = "Game";
		private const string ATTEMPT = "Attempt";
		private const string CHAR_IN_CORRECT_POSITION = "B";
		private const string CHAR_IN_WRONG_POSITION = "W";


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

			List<string> allCharsInDb = _charRepository.GetCharacter("w", true);
			List<string> newGame = allCharsInDb.OrderBy(x => Guid.NewGuid()).Take(4).ToList();  // Sorting the list in a random order using Guid.
																								// Then the first four items are picked.

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
                //string queryText = $@"INSERT INTO Game(GameTypeId, Value1, Value2, Value3, Value4)
                //					  VALUES('{gametypeId}', '{newGame[0]}', '{newGame[1]}', '{newGame[2]}', '{newGame[3]}');";

                string queryText = $@"INSERT INTO Game(CreatedDate)
                					  VALUES(datetime('now'));";

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

		public Game GetGame(int gameId)
		{
            var game = new Game();
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = @$"SELECT g.GameId, g.CreatedDate, g.ModifiedDate
									 FROM Game g
									 WHERE g.GameId = {gameId};";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);
				
				conn.Open();
				using (var reader = cmd.ExecuteReader())
				{
                    if (reader.Read())
                    {
                        game.GameId = Convert.ToInt32(reader["GameId"]);
                        game.CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString());
                        game.ModifiedDate = DateTime.Parse(reader["ModifiedDate"].ToString());
                    }
                }	
                conn.Close();
			}
			
			return game;
		}

		public bool RegisterAttempt(int gameId, List<string> attempt)
		{
			bool result = false;

			if (_charRepository.VerifyCharactersInGame(attempt))
			{
				using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
				{
					string queryText = $@"INSERT INTO Game(Value1, Value2, Value3, Value4)
									  VALUES('{attempt[0]}', '{attempt[1]}', '{attempt[2]}', '{attempt[3]}');";

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
