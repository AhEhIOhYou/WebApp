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
			ViewBag.StaffList = StaffContext.GetAllStaff();
			ViewBag.OperationTypeList = OperationTypeContext.GetAllOperationTypes();
			ViewBag.CashBoxList = CashBoxContext.GetAllCashBox();
			ViewBag.ContractsList = ContractContext.GetAllContracts();
			ViewBag.LogBookList = LogBookContext.GetAllLogBooks();
			return View();
		}
	}
}