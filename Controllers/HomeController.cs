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
		public ActionResult Index()
		{
			Staff s = new Staff();
			List<Staff> staffList = new List<Staff>(s.GetPerson());
			ViewBag.StaffList = staffList;
			return View();
		}
	}
}