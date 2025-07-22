using MasterMindResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MasterMindDataAccess
{
	public class CharactersRepository : ICharactersRepository
	{
		public CharactersRepository(string connectionString)
		{
			VALID_CHARACTERS = new List<string> { "A", "B", "C", "D", "E", "F" };
			_connectionString = connectionString;
		}

		private string _connectionString;

		private readonly List<string> VALID_CHARACTERS;
		private const int NUM_VALID_CHARACTERS = 6;


		private List<string> GetAllValidCharactersFromDB()
		{
			List<string> result = new List<string>();

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = @"SELECT Character
									 FROM ValidCharacters
									 ORDER BY Character ASC";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);
				conn.Open();
				SQLiteDataReader dr = cmd.ExecuteReader();

				while (dr.Read())
					result.Add(Convert.ToString(dr["Character"]));

				conn.Close();
			}

			return result;
		}

		private bool InsertCharToDB(string charToInsert)
		{
			bool result = true;

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = $@"INSERT INTO ValidCharacters(Character)
									  VALUES('{charToInsert}');";

				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

				conn.Open();
				cmd.ExecuteNonQuery();
				conn.Close();
			}

			return result;
		}

		public bool DeleteChar(string charToDelete, bool deleteAll = false)
		{
			bool result = true;

			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				string queryText = "DELETE FROM ValidCharacters";

				if (!deleteAll)
					queryText += $" WHERE Character = '{charToDelete}'";

				queryText += ";";
				SQLiteCommand cmd = new SQLiteCommand(queryText, conn);

				conn.Open();
				cmd.ExecuteNonQuery();
				conn.Close();
			}

			return result;
		}

		public List<string> GetCharacter(string charToGet, bool getAll = false)
		{
			List<string> allCharactersFromDB = GetAllValidCharactersFromDB();
			List<string> result = new List<string>();

			if (!getAll)
			{
				if (allCharactersFromDB.Contains(charToGet))
					result.Add(charToGet);
			}
			else
				result = allCharactersFromDB;

			return result;
		}

		public List<string> VerifyValidCharacters()
		{
			List<string> allCharactersInDB = GetAllValidCharactersFromDB();

			if(allCharactersInDB.Count != NUM_VALID_CHARACTERS)
			{
				foreach (string c in VALID_CHARACTERS)
				{
					if(!allCharactersInDB.Contains(c))
						InsertChar(c);
				}

				allCharactersInDB = GetAllValidCharactersFromDB();
			}

			return allCharactersInDB;
		}

		public void InsertChar(string charToInsert)
		{
			List<string> allCharactersFromDB = GetAllValidCharactersFromDB();

			if (!allCharactersFromDB.Contains(charToInsert))
				InsertCharToDB(charToInsert);
		}

		/// <summary>
		/// This method checks each of the four characters in the game, 
		/// to see if they represent valid characters for a game.
		/// </summary>
		/// <param name="game"></param>
		/// <returns></returns>
		public bool VerifyCharactersInGame(List<string> game)
		{
			bool result = true;

			foreach (string c in game)
			{
				if(!VALID_CHARACTERS.Contains(c))
				{
					result = false;
					break;
				}
			}

			return result;
		}
	}
}
