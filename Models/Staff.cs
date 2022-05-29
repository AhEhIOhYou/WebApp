using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class Staff
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class StaffContext : DbConx
	{
		public static List<Staff> GetAllStaff(string sort)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			if (sort == "")
				sort = "id";

			DbConx cn = new DbConx();
			if (!cn.CanSort("staff", sort))
			{
				return null;
			}

			List<Staff> staffList = new List<Staff>();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT * FROM staff ORDER BY {sort}";

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
		public static Staff GetStaffById(int userId)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			Staff staff = new Staff();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT * FROM staff WHERE id = {userId}";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					reader.Read();
					staff.Id = Int32.Parse(reader[0].ToString());
					staff.Name = reader[1].ToString();
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
			return staff;
		}
		public static bool Create(string userName)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;

				cmd.CommandText = $"INSERT INTO staff (name) VALUE ('{userName}')";
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
		public static bool Update(int id, string userName)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"UPDATE staff SET staff.name = '{userName}' WHERE staff.id='{id}'";
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
	}
}