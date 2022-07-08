using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MYapplication.Models
{
	public class Login_Page
	{
		[Required(ErrorMessage = "UserId is required")]
		public string UserId { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}

	public class Song
	{
		public int SongId { get; set; }
		public string SongName { get; set; }
		public string Singer { get; set; }
	}

	public class register
	{
		[Required(ErrorMessage = "UserName is required")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Password is required")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "UserName is required")]
		public string UserId { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
	public class create
	{
		public int SongId { get; set; }
		public string SongName { get; set; }
		public string Singer { get; set; }

	}
	public class Edit
	{
		public string OldSongName { get; set; }
		public string OldSinger { get; set; }
		public string SongName { get; set; }
		public string Singer { get; set; }

	}
	public class delete
	{
		public string SongName { get; set; }
		public string Singer { get; set; }
	}
}