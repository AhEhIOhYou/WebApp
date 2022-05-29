using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class OperationType
	{
		public int Id { get; set; }
		public string Type { get; set; }
	}
	
	public static class OperationTypeContext
	{
		public static List<OperationType> GetAllOperationTypes(string sort)
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
							Type = reader[1].ToString()
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
		public static OperationType GetOperationTypeById(int operationTypeId)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			OperationType ot = new OperationType();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT * FROM operation_type WHERE id = {operationTypeId}";

				MySqlDataReader reader = cmd.ExecuteReader();
				try
				{
					reader.Read();
					ot.Id = Int32.Parse(reader[0].ToString());
					ot.Type = reader[1].ToString();
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
			return ot;
		}
		public static bool Create(string operationType)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();

			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;

				cmd.CommandText = $"INSERT INTO operation_type (type) VALUE ('{operationType}')";
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
		public static bool Update(int id, string operationType)
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"UPDATE operation_type " +
								$"SET operation_type.type = '{operationType}' " +
								$"WHERE operation_type.id='{id}'";
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