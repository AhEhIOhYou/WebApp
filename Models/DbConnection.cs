using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public static class DbConnection
	{
		public static MySqlConnection Get_Connection()
		{
			var connection = new MySqlConnection();
			connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
			return connection;
		}
	}
}