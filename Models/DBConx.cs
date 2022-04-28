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

		public (List<string>, List<List<object>>) ExecuteSelect(string sqlText)
		{
			List<string> lCols = new List<string>();
			List<List<object>> lData = new List<List<object>>();
			List<object> tmp = new List<object>();
			
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = conn;
			cmd.CommandText = $"{sqlText}";
			MySqlDataReader reader = cmd.ExecuteReader();

			if(reader.HasRows) // если есть данные
			{
				int colCount = reader.FieldCount;
				for (int i = 0; i < colCount; i++)
				{
					lCols.Add(reader.GetName(i));
				}

				while (reader.Read()) // построчно считываем данные
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
			
			return (lCols, lData);
		}

		public string GetOpType(string sqlText) => sqlText.Split().First();
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