using System;
using System.Collections.Generic;
using System.Web.WebPages;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class Contract
	{
		public int Id { get; set; }
		
		public int IdUser { get; set; }
		public string NameUser { get; set; }
		
		public int IdType { get; set; }
		public string NameType { get; set; }
		
		public DateTime CDate { get; set; }
		public int Sum { get; set; }
	}
	
	public static class ContractContext
	{
		public static List<Contract> GetAllContracts(string sort)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			List<Contract> contractsList = new List<Contract>();
			if (sort == "")
				sort = "mydb.contract.id";
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = "SELECT mydb.contract.*, ot.type, s.name FROM mydb.contract " +
								"JOIN mydb.operation_type ot on ot.id = contract.id_type " +
								"JOIN mydb.staff s on s.id = contract.id_user " +
								$"ORDER BY {sort};";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					while (reader.Read())
					{
						contractsList.Add(new Contract()
						{
							Id = Int32.Parse(reader[0].ToString()),
							IdUser = Int32.Parse(reader[1].ToString()),
							IdType = Int32.Parse(reader[2].ToString()),
							CDate = reader[3].ToString().AsDateTime(),
							Sum = Int32.Parse(reader[4].ToString()),
							NameType = reader[5].ToString(),
							NameUser = reader[6].ToString()
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
			
			return contractsList;
		}
		public static Contract GetContractById(int contractId)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			Contract ct = new Contract();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT mydb.contract.*, ot.type, s.name FROM mydb.contract " +
								$"JOIN mydb.operation_type ot on ot.id = contract.id_type " +
								$"JOIN mydb.staff s on s.id = contract.id_user " +
								$"WHERE contract.id = {contractId}";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					reader.Read();
					ct.Id = Int32.Parse(reader[0].ToString());
					ct.IdUser = Int32.Parse(reader[1].ToString());
					ct.IdType = Int32.Parse(reader[2].ToString());
					ct.CDate = reader[3].ToString().AsDateTime();
					ct.Sum = Int32.Parse(reader[4].ToString());
					ct.NameType = reader[5].ToString();
					ct.NameUser = reader[6].ToString();
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
			return ct;
		}
		public static bool Create(int idUser, int idType, DateTime cDate, int sum)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				string date = cDate.ToString("yyyy-MM-dd HH:mm:ss");
				
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;

				cmd.CommandText = $"INSERT INTO mydb.contract " +
								$"(id_user, id_type, cdate, sum) " +
								$"VALUE ('{idUser}', '{idType}', '{date}', '{sum}')";
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
		public static bool Update(int id, int idUser, int idType, DateTime cDate, int sum)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			try
			{
				string date = cDate.ToString("yyyy-MM-dd HH:mm:ss");
				
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"UPDATE mydb.contract " +
								$"SET contract.id_user = '{idUser}', " +
								$"contract.id_type = '{idType}', " +
								$"contract.cdate = '{date}', " +
								$"contract.sum = '{sum}' " +
								$"WHERE contract.id='{id}'";
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
				cmd.CommandText = $"SELECT ct.id, s.name, ot.type, ct.cdate, ct.sum" +
								$" FROM mydb.contract as ct " +
								$"JOIN mydb.staff s on s.id = ct.id_user " +
								$"JOIN mydb.operation_type ot on ot.id = ct.id_type " +
								$"WHERE ct.cdate LIKE '{searchText}' " +
								$"OR ct.sum LIKE '{searchText}' " +
								$"OR ct.sum LIKE '{searchText}' " +
								$"OR s.name LIKE '{searchText}' " +
								$"OR ot.type LIKE '{searchText}' ";
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					for (int i = 0; i < 5; i++)
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