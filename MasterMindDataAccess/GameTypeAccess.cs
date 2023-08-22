using MasterMindResources.Interfaces;
using System;
using System.Data.SQLite;

namespace MasterMindDataAccess
{
	public class GameTypeAccess : IGameTypeAccess
	{
		public GameTypeAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

		string _connectionString;

		public int CreateGameType(string gameType)
		{
			int gameTypeId = 0;

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = @$"INSERT INTO GameType(gameType)
									  VALUES('{gameType}'); ";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);
				conn.Open();
				cmd.ExecuteNonQuery();

				// Now, we fetch the id of the newly inserted gameType. 
				// We could use the number of rows affected to verify if insertion succeeded. To be decided...
				queryText = @$"SELECT t.GameTypeId
							   FROM GameType t
							   WHERE t.GameType = '{gameType}'; ";

				cmd = new SQLiteCommand(queryText, conn);
				object result = cmd.ExecuteScalar();
				gameTypeId = Convert.ToInt32(result);

				conn.Close();
			}

			return gameTypeId;
		}

		public string GetGameType(int gameTypeId)
		{
			string gameType = string.Empty;

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = @$"SELECT t.GameType
									  FROM GameType t
									  WHERE t.GameTypeId = '{gameTypeId}'; ";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

				conn.Open();

				object result = cmd.ExecuteScalar();
				gameType = (result != null) ? result.ToString() : string.Empty;

				conn.Close();
			}

			return gameType;
		}

		public int GetGameTypeIdByGameType(string gameType)
		{
			int gameTypeId = 0;

			using(SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = @$"SELECT t.GameTypeId
									  FROM GameType t
									  WHERE t.GameType = '{gameType}'; ";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

				conn.Open();

				object result = cmd.ExecuteScalar();
				gameTypeId = Convert.ToInt32(result);

				conn.Close();
			}

			return gameTypeId;
		}
	}
}
