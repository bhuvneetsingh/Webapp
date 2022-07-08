using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using MYapplication.Models;
using WebApplication5.Helpers;

namespace WebApplication5.Controllers
{
	public class HomeController : Controller
	{
		string ConnectionString = @"Data Source=LAPTOP-FU6E3UJ8\SQLEXPRESS;Initial Catalog=bhuvneet;Trusted_Connection=true";
		SqlConnection cnn;


		public ActionResult index()
		{
			ViewBag.Message = "For new entry go to registration, for old customers go to login";
			return View();
		}
		public ActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Register(register model)
		{

			cnn = new SqlConnection(ConnectionString);
			cnn.Open();
			SqlCommand command;
			SqlDataAdapter adapter = new SqlDataAdapter();
			string sql = "";
			

			sql = " INSERT INTO [dbo].[Users]  ([UserId],[PassWord],[FirstName],[LastName]) Values('" + Convert.ToString(model.UserId) + "','" + Convert.ToString(model.Password) + "','" + Convert.ToString(model.FirstName) + "','" + Convert.ToString(model.LastName) + "')";
			command = new SqlCommand(sql, cnn);

			adapter.InsertCommand = new SqlCommand(sql, cnn);
			adapter.InsertCommand.ExecuteNonQuery();
			command.Dispose();
			cnn.Close();


			return Redirect("index");
		}
		public ActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Login(Login_Page model)
		{
			var userId = "";
			var password = "";
			cnn = new SqlConnection(ConnectionString);
			cnn.Open();
			SqlCommand command;
			SqlDataReader dataReader;
			string sql;
			sql = " select * from [dbo].[Users]";
			command = new SqlCommand(sql, cnn);
			dataReader = command.ExecuteReader();
			while (dataReader.Read() == true)
			{
				userId = Convert.ToString(dataReader.GetValue(0));
				password = Convert.ToString(dataReader.GetValue(3));

				if (model.UserId == userId && model.Password == password)
				{
					Session["UserId"] = model.UserId;
					return Redirect("UserLandingView");
				}
			}
			command.Dispose();
			cnn.Close();
			return View();
		}


		public ActionResult UserLandingView()
		{
			Song model;
			Login_Page model1;
			model1 = new Login_Page();
		    IList<Song> result = new List<Song>();
			cnn = new SqlConnection(ConnectionString);
			cnn.Open();
			SqlCommand command;
			SqlDataReader dataReader;
			string sql;
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
			sql = " select * from [dbo].[Songs] inner join [dbo].[UserSongMap] on [dbo].[Songs].[Id] = [dbo].[UserSongMap].[SongId] where [dbo].[UserSongMap].[UserId] ='" + model1.UserId + "'";
			command = new SqlCommand(sql, cnn);
			dataReader = command.ExecuteReader();
			while (dataReader.Read())
			{

				model = new Song();
				model.SongName = Convert.ToString(dataReader.GetValue(1));
				model.Singer = Convert.ToString(dataReader.GetValue(2));
				result.Add(model);
			}
			command.Dispose();
			cnn.Close();
			Session["list"] = result;
			return View(result);
		}

		public ActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Create(create model)
		{
			Login_Page model1;
			List<create> res = new List<create>();
			cnn = new SqlConnection(ConnectionString);
			cnn.Open();
			SqlCommand command;
			SqlDataAdapter adapter = new SqlDataAdapter();
			SqlDataReader dataReader;
			string sql, sql1, sql2 = "";
			sql = " INSERT INTO [dbo].[Songs]  ([SongName],[Singer]) Values('" + Convert.ToString(model.SongName) + "','" + Convert.ToString(model.Singer) + "')";
			command = new SqlCommand(sql, cnn);
			Session["SongName"] = model.SongName;
			adapter.InsertCommand = new SqlCommand(sql, cnn);
			adapter.InsertCommand.ExecuteNonQuery();
			command.Dispose();

			sql1 = " select [Id] from [dbo].[Songs] where [dbo].[Songs].[SongName]='" + model.SongName + "'";
			command = new SqlCommand(sql1, cnn);
			dataReader = command.ExecuteReader();
			while (dataReader.Read())
			{
				model.SongId = Convert.ToInt32(dataReader.GetValue(0));
				res.Add(model);
			}
			dataReader.Close();
			command.Dispose();
			model1 = new Login_Page();
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
			sql2 = "insert into [dbo].[UserSongMap] ([UserId],[SongId]) values( '" + Convert.ToInt32(model1.UserId) + "','" + model.SongId + "')";
			command = new SqlCommand(sql2, cnn);

			adapter.InsertCommand = new SqlCommand(sql2, cnn);
			adapter.InsertCommand.ExecuteNonQuery();
			command.Dispose();
			cnn.Close();
				return View("uploadFile");
			
		}
		public ActionResult Edit(string id)
		{
			SongOperations model;
			Login_Page model1;
			model1 = new Login_Page();
			model = new SongOperations();
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
			var std = model.GetSongs(Convert.ToInt32(model1.UserId)).Where(s => s.SongName == id).FirstOrDefault();
			Session["song"] = std.SongName;
			return View(std);
		}
		[HttpPost]
		public ActionResult Edit(Song std)
		{
			var Song = " ";
			SongOperations model;
			Login_Page model1;
			model1 = new Login_Page();
			model = new SongOperations();
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
			if (Session["song"] != null)
				Song = Session["song"].ToString();
			var student = model.GetSongs(Convert.ToInt32(model1.UserId)).Where(s => s.SongName == Song).FirstOrDefault();

			cnn = new SqlConnection(ConnectionString);
			cnn.Open();
			SqlCommand command;
			SqlDataAdapter adapter = new SqlDataAdapter();
			string sql = " ";
			sql = " update [dbo].[Songs] set [SongName] =  '" + std.SongName + "' , [Singer] = '" + std.Singer + "'  where SongName = ('" + Song + "')";
			command = new SqlCommand(sql, cnn);
			adapter.UpdateCommand = new SqlCommand(sql, cnn);
			adapter.UpdateCommand.ExecuteNonQuery();
			command.Dispose();
			cnn.Close();

			return RedirectToAction("UserLandingView");
		}
		public ActionResult Delete(string id)
		{
			SongOperations model;
			Login_Page model1;
			model1 = new Login_Page();
			model = new SongOperations();
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
			var std = model.GetSongs(Convert.ToInt32(model1.UserId)).Where(s => s.SongName == id).FirstOrDefault();
			Session["name"]= std.SongName;
			return View(std);
		}
		[HttpPost]
		public ActionResult Delete(Song std)
		{
			var Name = "";
			Song model;
			model = new Song();
			List<Song> res = new List<Song>();
			cnn = new SqlConnection(ConnectionString);
			if (Session["name"] != null)
				Name = Session["name"].ToString();
			cnn.Open();
			SqlCommand command;
			SqlDataAdapter adapter = new SqlDataAdapter();
			SqlDataReader reader;
			string sql, sql1, sql2 = " ";
			sql = " select [Id] from [dbo].[Songs] where [SongName]='" + Name + "'";
			command = new SqlCommand(sql, cnn);
			reader = command.ExecuteReader();
			while (reader.Read())
			{
				 model.SongId = Convert.ToInt32(reader.GetValue(0));
				 res.Add(model);
			}

			reader.Close();
			command.Dispose();

			sql1 = " Delete from [dbo].[Songs] where [SongName] = '" + std.SongName + "'";
			command = new SqlCommand(sql1, cnn);
			adapter.DeleteCommand = new SqlCommand(sql1, cnn);
			adapter.DeleteCommand.ExecuteNonQuery();
			command.Dispose();

			sql2 = "  delete from [dbo].[UserSongMap] where [SongId] = '" + model.SongId + "'";
			command = new SqlCommand(sql2, cnn);
			adapter.DeleteCommand = new SqlCommand(sql2, cnn);
			adapter.DeleteCommand.ExecuteNonQuery();
			command.Dispose();
			cnn.Close();


			return RedirectToAction("UserLandingView");
		}
		[HttpGet]
		public ActionResult UploadFile(string id)
		{
			SongOperations model;
			Login_Page model1;
			model1 = new Login_Page();
			model = new SongOperations();
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
			var std =model.GetSongs(Convert.ToInt32(model1.UserId)).Where(s => s.SongName == id);
			return View(std);
		}
		[HttpPost]
		public ActionResult UploadFile(HttpPostedFileBase file)
		{
			SongOperations model2;
			model2 = new SongOperations();
			create model;
			Login_Page model1;
			model = new create();
			model1 = new Login_Page();
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
			if (Session["SongName"] != null)
				model.SongName = Session["SongName"].ToString();
			var std = model2.GetSongs(Convert.ToInt32(model1.UserId)).Where(s => s.SongName == model.SongName ).FirstOrDefault();
			try
			{
				if (file != null && file.ContentLength > 0)
				{
					string fileName = model1.UserId + "-" + std.SongName + ".mp3" ;

					string path = Path.Combine(Server.MapPath("~/App_Data/UploadedFiles"),fileName);
					file.SaveAs(path);
					
				}
				ViewBag.Message = "File Uploaded Successfully!!";
				return RedirectToAction("UserLandingView");
			}
			


			catch
			{
				ViewBag.Message = "File upload failed!!";
				return View();
			}
		}

		[HttpGet]
	    
		public ActionResult GetSong(string id, string dt = null)
		{
			
			SongOperations model;
		    Login_Page model1;
		    model = new SongOperations();
		    model1 = new Login_Page();
		
			if (Session["UserId"] != null)
				model1.UserId = Session["UserId"].ToString();
		   
			string filename = model1.UserId + "-"+ id +".mp3";
			string filepath = @"C:\Users\bhuvn\source\repos\WebApplication5\WebApplication5\App_Data\UploadedFiles\" + filename + "";
			byte[] filedata = System.IO.File.ReadAllBytes(filepath);
			string contentType = MimeMapping.GetMimeMapping(filepath);

			var cd = new System.Net.Mime.ContentDisposition
			{
				FileName = filename,
				Inline = true,
			};

			Response.AppendHeader("Content-Disposition", cd.ToString());


			return File(filedata, contentType);
		}
		public ActionResult EndPage()
		{
			ViewBag.Message = " Thanks for Visiting";
			return View();
		}
	}
}