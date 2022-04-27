using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class DbConx
	{
		public virtual bool DeleteById(string table, int id)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"DELETE FROM {table} WHERE id = {id}";
				cmd.ExecuteReader();
				conn.Close();
				return true;
			}
			catch (MySqlException e)
			{
				conn.Close();
				return false;
			}
		}

		public string ExecuteCommand(string text)
		{
			string mainAction = text.Split().Last();
			return mainAction;
		}

		public List<string> GetStructure(string table)
		{
			List<string> result = new List<string>();
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"SHOW COLUMNS FROM {table}";

			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				result.Add(reader[0].ToString());
			}

			conn.Close();
			return result;
		}
	}
}