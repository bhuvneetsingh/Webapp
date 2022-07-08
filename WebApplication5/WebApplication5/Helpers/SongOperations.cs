using MYapplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication5.Helpers
{
	

	public class SongOperations {
		string ConnectionString = @"Data Source=LAPTOP-FU6E3UJ8\SQLEXPRESS;Initial Catalog=bhuvneet;Trusted_Connection=true";
		SqlConnection cnn;
		public IList<Song> GetSongs(int UserId)
		{
			IList<Song> result = new List<Song>();
			cnn = new SqlConnection(ConnectionString);
			cnn.Open();
			SqlCommand command;
			SqlDataReader dataReader;
			string sql;
			sql = " select * from [dbo].[Songs] inner join [dbo].[UserSongMap] on [dbo].[Songs].[Id] = [dbo].[UserSongMap].[SongId] where [dbo].[UserSongMap].[UserId] ='" + UserId + "'";
			command = new SqlCommand(sql, cnn);
			dataReader = command.ExecuteReader();
			while (dataReader.Read())
			{
				result.Add(new Song { SongName = Convert.ToString(dataReader.GetValue(1)), Singer = Convert.ToString(dataReader.GetValue(2)) });
			}
			command.Dispose();
			cnn.Close();
			return result;
		}
		
	}
}