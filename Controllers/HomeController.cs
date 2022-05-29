using System;
using Microsoft.AspNetCore.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class HomeController : Controller
	{
		private DbConx context = new DbConx();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Signup()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Signup(User user)
		{
			ViewBag.Status = Auth.SignUp(user.Login, user.Pass, user.Name, user.Role);
			if (ViewBag.Status == -1)
				return View();
			return RedirectToAction("Signin");
		}
		public ActionResult Signin()
		{
			var user = Auth.GetUserSession();
			if (user != null)
			{
				return RedirectToAction("Index");
			}
			return View();
		}
		[HttpPost]
		public ActionResult Signin(User user)
		{
			ViewBag.Status = Auth.SignIn(user.Login, user.Pass);
			
			if (ViewBag.Status != 1)
				return View();
			
			return RedirectToAction("Index");
		}

		public ActionResult Account()
		{
			var user = Auth.GetUserSession();
			if (user == null)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		[HttpPost]
		public ActionResult Account(User user)
		{
			var userSession = Auth.GetUserSession();
			if (userSession == null)
			{
				return RedirectToAction("Index");
			}

			UserContext.Update(user.Id, user.Name);
			userSession.Name = user.Name;
			
			return View();
		}

		public ActionResult Logout()
		{
			Auth.LogOut();
			return RedirectToAction("Index");
		}

		public ActionResult Sql()
		{
			ViewBag.Struct = null;
			ViewBag.ItemsRow = null;
			ViewBag.SqlText = "";
			ViewBag.SqlResponse = null;
			
			var user = Auth.GetUserSession();
			if (user == null)
			{
				return RedirectToAction("Index");
			}

			if (Request.QueryString["sqlquery"] != null && Request.QueryString["sqlquery"] != "")
			{
				string sqlText = Request.QueryString["sqlquery"];
				
				if (context.GetOpType(sqlText).ToLower() == "select")
				{
					var tp = context.ExecuteSelect(sqlText);
					if (tp.Item1 == null && tp.Item2 == null)
					{
						ViewBag.SqlResponse = context.ExecuteCommand(sqlText);
					}
					ViewBag.Struct = tp.Item1;
					ViewBag.ItemsRow = tp.Item2;
				}
				else
				{
					ViewBag.SqlResponse = context.ExecuteCommand(sqlText);
				}
				
				ViewBag.SqlText = sqlText;
			}
			
			return View();
		}
	}
}