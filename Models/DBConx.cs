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

		// Не работает
		public List<string> GetCols(string table)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT `COLUMN_NAME` FROM `INFORMATION_SCHEMA`.`COLUMNS` " +
								$"WHERE `TABLE_SCHEMA`='mydb' AND `TABLE_NAME`='staff';";

				MySqlDataReader reader = cmd.ExecuteReader();
				
			}
			catch (MySqlException e)
			{
				conn.Close();
				return null;
			}

			conn.Close();
			return null;
		}

		public bool CanSort(string tableName, string sortParams)
		{
			bool can = false;
			
			sortParams = sortParams.Replace(" ", "");
			sortParams = sortParams.Replace("ASC", "");
			sortParams = sortParams.Replace("DESC", "");
			string[] words = sortParams.Split(',');

			foreach (var word in words)
			{
				switch (tableName)
				{
					case "staff":
						if (word == "id" || word == "name")
							can = true;
						else
							can = false;
						break;
					case "operation_type":
						if (word == "id" || word == "type")
							can = true;
						else
							can = false;
						break;
					case "logbook":
						if (word == "id" || word == "id_cashbox" || word == "id_contract" || word == "ldate")
							can = true;
						else
							can = false;
						break;
					case "contract":
						if (word == "id" || word == "id_user" || word == "id_type" || word == "cdate" || word == "sum")
							can = true;
						else
							can = false;
						break;
					case "cashbox":
						if (word == "id" || word == "name")
							can = true;
						else
							can = false;
						break;
				}
			}
			return can;
		}
	}
}