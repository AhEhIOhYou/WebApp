using System;
using System.Collections.Generic;
using System.Web.WebPages;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class LogBook
	{
		public int Id { get; set; }
		
		public int IdCashBox { get; set; }
		public string NameCashBox { get; set; }
		
		public int IdContract { get; set; }
		public DateTime LDate { get; set; }
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
				cmd.CommandText = "SELECT lb.*, c.name FROM logbook as lb " +
									"JOIN cashbox c on c.id = lb.id_cashbox " +
									"ORDER BY lb.id";

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
							LDate = reader[3].ToString().AsDateTime(),
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
		public static LogBook GetLogBookById(int logBookId)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			LogBook lb = new LogBook();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = "SELECT lb.*, c.name FROM logbook as lb " +
								"JOIN cashbox c on c.id = lb.id_cashbox " +
								$"WHERE lb.id = {logBookId}";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					reader.Read();
					lb.Id = Int32.Parse(reader[0].ToString());
					lb.IdCashBox = Int32.Parse(reader[1].ToString());
					lb.IdContract = Int32.Parse(reader[2].ToString());
					lb.LDate = reader[3].ToString().AsDateTime();
					lb.NameCashBox = reader[4].ToString();
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
			return lb;
		}
		public static bool Create(int idCashBox, int idContract, DateTime lDate)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				
				string date = lDate.ToString("yyyy-MM-dd HH:mm:ss");

				cmd.CommandText = "INSERT INTO logbook (id_cashbox, id_contract, ldate ) " +
								$"VALUES ('{idCashBox}', '{idContract}', '{date}')";
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
		public static bool Update(int id, int idCashBox, int idContract, DateTime lDate)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				string date = lDate.ToString("yyyy-MM-dd HH:mm:ss");
				cmd.CommandText = $"UPDATE logbook SET " +
								$"logbook.id_cashbox = '{idCashBox}', " +
								$"logbook.id_contract = '{idContract}', " +
								$"logbook.ldate = '{date}' " +
								$"WHERE logbook.id='{id}'";
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
				cmd.CommandText = $"SELECT lb.id, cb.name, lb.id_contract, lb.ldate " +
								$"FROM mydb.logbook as lb " +
								$"JOIN mydb.cashbox cb on cb.id = lb.id_cashbox = cb.id " +
								$"WHERE lb.ldate LIKE '{searchText}' " +
								$"OR lb.id_contract LIKE '{searchText}' " +
								$"OR cb.name LIKE '{searchText}' ";
								MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					for (int i = 0; i < 4; i++)
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