using System;
using System.Collections.Generic;
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
		
		public string CDate { get; set; }
		public int Sum { get; set; }
	}
	
	public static class ContractContext
	{
		public static List<Contract> GetAllContracts()
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			List<Contract> contractsList = new List<Contract>();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = String.Format("SELECT contract.*, ot.type, s.name FROM contract " +
												"JOIN operation_type ot on ot.id = contract.id_type " +
												"JOIN staff s on s.id = contract.id_user");

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
							CDate = reader[3].ToString(),
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
	}
}