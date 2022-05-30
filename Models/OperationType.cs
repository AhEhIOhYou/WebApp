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
			
			if (sort == "")
				sort = "id";
			
			List<OperationType> operationTypeList = new List<OperationType>();
			try
			{
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = $"SELECT * FROM operation_type ORDER BY {sort}";

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
				cmd.CommandText = $"SELECT ot.id, ot.type " +
								$"FROM mydb.operation_type as ot " +
								$"WHERE ot.type LIKE '{searchText}' ";
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					for (int i = 0; i < 2; i++)
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