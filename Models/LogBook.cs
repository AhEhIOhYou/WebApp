using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class LogBook
	{
		public int Id { get; set; }
		
		public int IdCashBox { get; set; }
		public string NameCashBox { get; set; }
		
		public int IdContract { get; set; }
		public string LDate { get; set; }
	}

	public static class LogBookContext
	{
		public static List<LogBook> GetAllLogBooks()
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			List<LogBook> logBooksList = new List<LogBook>();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = String.Format("SELECT lb.*, c.name FROM logbook as lb " +
												"JOIN cashbox c on c.id = lb.id_cashbox " +
												"ORDER BY lb.id");

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					while (reader.Read())
					{
						logBooksList.Add(new LogBook()
						{
							Id = Int32.Parse(reader[0].ToString()),
							IdCashBox = Int32.Parse(reader[1].ToString()),
							IdContract = Int32.Parse(reader[2].ToString()),
							LDate = reader[3].ToString(),
							NameCashBox = reader[4].ToString(),
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
			return logBooksList;
		}
	}
}