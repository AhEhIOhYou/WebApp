using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class OperationType
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
	
	public static class OperationTypeContext
	{
		public static List<OperationType> GetAllOperationTypes()
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			List<OperationType> operationTypeList = new List<OperationType>();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = String.Format("SELECT * FROM operation_type");

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					while (reader.Read())
					{
						operationTypeList.Add(new OperationType
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
			return operationTypeList;
		}
	}
}