using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class Staff
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public static class StaffContext
	{
		public static List<Staff> GetAllStaff()
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			List<Staff> staffList = new List<Staff>();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = String.Format("SELECT * FROM staff");

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					while (reader.Read())
					{
						staffList.Add(new Staff
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
			return staffList;
		}
	}
}