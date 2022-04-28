using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
		
		public ActionResult Sql()
		{
			ViewBag.Struct = null;
			ViewBag.ItemsRow = null;
			ViewBag.SqlText = "";
			
			if (Request.QueryString["sqlquery"] != null && Request.QueryString["sqlquery"] != "")
			{
				var tp = context.ExecuteSelect(Request.QueryString["sqlquery"]);
				ViewBag.Struct = tp.Item1;
				ViewBag.ItemsRow = tp.Item2;
				ViewBag.SqlText = Request.QueryString["sqlquery"];
			}
			
			return View();
		}
	}
}