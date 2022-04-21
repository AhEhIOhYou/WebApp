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

	public static class CashBoxContext
	{
		public static List<CashBox> GetAllCashBox()
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			List<CashBox> cashBoxList = new List<CashBox>();
			
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = String.Format("SELECT * FROM cashbox");

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
	}
}