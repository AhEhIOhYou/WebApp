using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Pass { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
		public string Salt { get; set; }
	}
	
	public class UserContext : DbConx
	{
		public static User GetUser(string login)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			User user = new User();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT * FROM mydb.users WHERE login = '{login}'";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					reader.Read();
					user.Id = Int32.Parse(reader[0].ToString());
					user.Login = reader[1].ToString();
					user.Pass = reader[2].ToString();
					user.Name = reader[3].ToString();
					user.Role =  reader[4].ToString();
					user.Salt = reader[5].ToString();
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
			return user;
		}
		public static bool Create(string login, string pass, string userName, string Role, string salt)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			var role = (Role == null) ? "def" : "admin";

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;

				cmd.CommandText = $"INSERT INTO mydb.users (login, pass, name, role, salt) VALUE ('{login}', '{pass}','{userName}', '{role}', '{salt}')";
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
				cmd.CommandText = $"UPDATE mydb.users SET users.name = '{userName}' WHERE users.id='{id}'";
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