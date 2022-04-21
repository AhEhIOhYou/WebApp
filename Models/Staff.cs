using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
	public class Staff
	{
		public int Id { get; set; } 
		public string Name { get; set; }


		public List<Staff> GetPerson()
		{
			MySqlConnection conn = DbConnection.Get_Connection();
			conn.Open();
			
			List<Staff> pLsit = new List<Staff>();
			
			try
			{
				Id = 2;
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = conn;
				cmd.CommandText = String.Format("SELECT * FROM staff");

				MySqlDataReader reader = cmd.ExecuteReader();

				try
				{
					while (reader.Read())
					{
						var id = Int32.Parse(reader[0].ToString());
						var name = reader[1].ToString();
						pLsit.Add(new Staff {Id = id, Name = name});
					}
					
		            reader.Close();

				}
				catch (MySqlException e)
				{
					string  MessageString = "Read error occurred  / entry not found loading the Column details: " + e.ErrorCode + " - " + e.Message + "; \n\nPlease Continue";
					reader.Close();
					Name = MessageString;
				}
				
			}
			catch (MySqlException e)
			{
					string  MessageString = "The following error occurred loading the Column details: " + e.ErrorCode + " - " + e.Message;
					Name = MessageString;
			}
			conn.Close();
			return pLsit;
		}

        
	}
}