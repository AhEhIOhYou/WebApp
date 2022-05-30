using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class CashBox
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class CashBoxContext : DbConx
	{
		public static List<CashBox> GetAllCashBox(string sort)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			List<CashBox> cashBoxList = new List<CashBox>();
			if (sort == "")
				sort = "id";
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT * FROM cashbox ORDER BY {sort}";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					while (reader.Read())
					{
						cashBoxList.Add(new CashBox
						{
							Id = Int32.Parse(reader[0].ToString()),
							Name = reader[1].ToString()
						});
					}

					reader.Close();
				}
				catch (MySqlException e)
				{
					reader.Close();
					return null;
				}

			}
			catch (MySqlException e)
			{
				return null;
			}

			conn.Close();
			return cashBoxList;
		}
		public static CashBox GetCashBoxById(int cashBoxId)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			CashBox cashBox = new CashBox();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT * FROM cashbox WHERE id = {cashBoxId}";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					reader.Read();
					cashBox.Id = Int32.Parse(reader[0].ToString());
					cashBox.Name = reader[1].ToString();
					reader.Close();
				}
				catch (MySqlException e)
				{
					reader.Close();
					return null;
				}
			}
			catch (MySqlException e)
			{
				return null;
			}

			conn.Close();
			return cashBox;
		}
		public static bool Create(string cashBoxName)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;

				cmd.CommandText = $"INSERT INTO cashbox (name) VALUE ('{cashBoxName}')";
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
		public static bool Update(int id, string cashBoxName)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"UPDATE cashbox SET cashbox.name = '{cashBoxName}' WHERE cashbox.id='{id}'";
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
		public static List<List<string>> Search(string searchText)
		{
			List<List<string>> result = new List<List<string>>();
			List<string> tmp = new List<string>();

			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT cb.id, cb.name " +
								$"FROM mydb.cashbox as cb " +
								$"WHERE cb.name LIKE '{searchText}' ";
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					for (int i = 0; i < 2; i++)
					{
						tmp.Add(reader.GetValue(i).ToString());
					}
					result.Add(tmp);
					tmp = new List<string>();
				}
				reader.Close();
			}
			catch (MySqlException e)
			{
				conn.Close();
				return null;
			}

			return result;
		}
	}
}