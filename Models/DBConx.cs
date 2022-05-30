using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class DbConx
	{
		public bool DeleteById(string table, int id)
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

		public (List<string>, List<List<object>>) ExecuteSelect(string sqlText)
		{
			List<string> lCols = new List<string>();
			List<List<object>> lData = new List<List<object>>();
			List<object> tmp = new List<object>();

			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"{sqlText}";
				MySqlDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					int colCount = reader.FieldCount;

					for (int i = 0; i < colCount; i++)
					{
						lCols.Add(reader.GetName(i));
					}

					while (reader.Read())
					{
						for (int i = 0; i < colCount; i++)
						{
							tmp.Add(reader.GetValue(i));
						}

						lData.Add(tmp);
						tmp = new List<object>();
					}
				}

				conn.Close();
			}
			catch (MySqlException e)
			{
				conn.Close();
				return (null, null);
			}

			return (lCols, lData);
		}

		public string ExecuteCommand(string sqlText)
		{
			string response = GetOpType(sqlText).ToLower() + ": ";
			int count = 0;
						
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"{sqlText}";
				MySqlDataReader reader = cmd.ExecuteReader();

				reader.Read();
				count = reader.RecordsAffected;
			}
			catch (MySqlException e)
			{
				conn.Close();
				return "Traceback: " + e;
			}

			conn.Close();
			return response + count + " row(s)";
		}

		public string GetOpType(string sqlText) => sqlText.Split().First();
	}
}