using MasterMindResources;
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

			
		}

		string _connectionString;
		ICharactersRepository _charRepository;
		
		private const string GAME = "Game";
		private const string ATTEMPT = "Attempt";
		

		public int CreateGame()
		{
			int gameId = 0;

            var allCharsInDb = _charRepository.GetCharacter("w", true);
            var attempt = allCharsInDb.OrderBy(x => Guid.NewGuid()).Take(4).ToList();  // Sorting the list in a random order using Guid.
                                                                                       // Then the first four items are picked.

            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
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

			RegisterAttempt(gameId, attempt, AttemptType.Solution);

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

		public int RegisterAttempt(int gameId, List<string> attempt, AttemptType attemptType = AttemptType.Attempt)
		{
            var attemptId = 0;
            var attemptTypes = GetAttemptTypes();
            var attemptTypeId = attemptTypes.Where(a => a.Value == attemptType).FirstOrDefault().Key;
            if (_charRepository.VerifyCharactersInGame(attempt))
			{
				using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
				{
                    var queryText = $@"INSERT INTO Attempt(GameId, ValueOne, ValueTwo, ValueThree, ValueFour, AttemptTypeId)
									   VALUES({gameId}, '{attempt[0].ToUpper()}', '{attempt[1].ToUpper()}', '{attempt[2].ToUpper()}', '{attempt[3].ToUpper()}', {attemptTypeId});";

					var cmd = new SQLiteCommand(queryText, conn);
					conn.Open();
                    gameId = Convert.ToInt32(cmd.ExecuteScalar());

                    queryText = "SELECT last_insert_rowid();";
                    cmd = new SQLiteCommand(queryText, conn);
                    attemptId = Convert.ToInt32(cmd.ExecuteScalar());
					conn.Close();
                }
			}
			else
				attemptId = 0;

			return attemptId;
		}

        public Dictionary<int, AttemptType> GetAttemptTypes()
        {
            var attemptTypes =  new Dictionary<int, AttemptType>();
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                string queryText = @$"SELECT a.AttemptTypeId, a.AttemptType
									  FROM AttemptType a
									  ORDER BY a.AttemptType DESC;";

                SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        attemptTypes.Add(int.Parse(reader[0].ToString()), Enum.Parse<AttemptType>(reader[1].ToString()));
                }
                conn.Close();
            }

            return attemptTypes;
        }

        public Attempt GetSolution(int gameId)
        {
            var attempts = GetAttempts(gameId)
				.Where(a => a.AttemptType == AttemptType.Solution);


            return attempts.ElementAt(0);
        }

        public AttemptType GetAttemptType(int attempId)
        {
			AttemptType attemptType = AttemptType.NotAttempted;
			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = @$"SELECT att.AttemptType
									  FROM Attempt a
									  JOIN AttemptType att ON att.AttemptTypeId = a.AttemptTypeId
									  WHERE a.AttemptId = {attempId};";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);
				conn.Open();

                attemptType = Enum.Parse<AttemptType>(cmd.ExecuteScalar().ToString());

				conn.Close();
			}

			return attemptType;
        }

        public List<Attempt> GetAttempts(int gameId)
        {
			var attempts = new List<Attempt>();
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
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

			return attempts;
        }

        public Attempt GetAttempt(int attemptId)
        {
            var attempt = new Attempt();

            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                string queryText = @$"SELECT a.GameId, a.ValueOne, a.ValueTwo, a.ValueThree, a.ValueFour, a.AttemptId, att.AttemptType
									 FROM Attempt a
									 JOIN AttemptType att ON att.AttemptTypeId = a.AttemptTypeId
									 WHERE a.AttemptId = {attemptId};";

                SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        attempt.GameId = Convert.ToInt32(reader["GameId"]);
                        attempt.AttemptId = Convert.ToInt32(reader["AttemptId"]);
                        attempt.ValueOne = reader["ValueOne"].ToString().ToUpper();
                        attempt.ValueTwo = reader["ValueTwo"].ToString().ToUpper();
                        attempt.ValueThree = reader["ValueThree"].ToString().ToUpper();
                        attempt.ValueFour = reader["ValueFour"].ToString().ToUpper();
                        attempt.AttemptType = Enum.Parse<AttemptType>(reader["AttemptType"].ToString());
                    }
                }
                conn.Close();
            }

            return attempt;
        }

        public bool SaveAttempt(Attempt attempt)
        {
            var result = false;

            if (_charRepository.VerifyCharactersInGame(attempt.ValuesList))
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    var queryText = $@"UPDATE Attempt
									   SET ValueOne   = '{attempt.ValueOne.ToUpper()}', 
										   ValueTwo   = '{attempt.ValueTwo.ToUpper()}', 
										   ValueThree = '{attempt.ValueThree.ToUpper()}',
										   ValueFour  = '{attempt.ValueFour.ToUpper()}',
										   ModifiedDate = datetime('now')
									   WHERE AttemptId = {attempt.AttemptId}";

                    var cmd = new SQLiteCommand(queryText, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

					result = true;
                }
            }

            return result;
        }

        
    }
}
