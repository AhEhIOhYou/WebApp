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
			if (Request.QueryString["delete"] != null  && Request.QueryString["table"] != null)
			{
				context.DeleteById(
					Request.QueryString["table"],
					Int32.Parse(Request.QueryString["delete"])
				);
			}

			ViewBag.StaffList = StaffContext.GetAllStaff();
			ViewBag.OperationTypeList = OperationTypeContext.GetAllOperationTypes();
			ViewBag.CashBoxList = CashBoxContext.GetAllCashBox();
			ViewBag.ContractsList = ContractContext.GetAllContracts();
			ViewBag.LogBookList = LogBookContext.GetAllLogBooks();
			
			return View();
		}

		[HttpGet]
		public ActionResult EditStaff(int id)
		{
			ViewBag.Staff = StaffContext.GetStaffById(id);
			return View();
		}
		[HttpPost]
		public ActionResult EditStaff(Staff s)
		{
			StaffContext.Update(s.Id, s.Name);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult EditOperationType(int id)
		{
			ViewBag.OperationType = OperationTypeContext.GetOperationTypeById(id); 
			return View();
		}
		[HttpPost]
		public ActionResult EditOperationType(OperationType ot)
		{
			OperationTypeContext.Update(ot.Id, ot.Type);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult EditContract(int id)
		{
			ViewBag.StaffList = StaffContext.GetAllStaff();
			ViewBag.OperationType = OperationTypeContext.GetAllOperationTypes();
			ViewBag.Contract = ContractContext.GetContractById(id); 
			return View();
		}
		[HttpPost]
		public ActionResult EditContract(Contract c)
		{
			ContractContext.Update(c.Id, c.IdUser, c.IdType, c.CDate, c.Sum);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public ActionResult EditLogBook(int id)
		{
			ViewBag.CashBoxList = CashBoxContext.GetAllCashBox();
			ViewBag.ContractList = ContractContext.GetAllContracts();
			ViewBag.LogBook = LogBookContext.GetLogBookById(id); 
			return View();
		}
		[HttpPost]
		public ActionResult EditLogBook(LogBook lb)
		{
			LogBookContext.Update(lb.Id, lb.IdCashBox, lb.IdContract, lb.LDate);
			return RedirectToAction("Index");
		}
		
		[HttpGet] 
		public ActionResult EditCashBox(int id)
		{
			ViewBag.CashBox = CashBoxContext.GetCashBoxById(id);
        	return View();
        }
        [HttpPost]
        public ActionResult EditCashBox(CashBox cb)
		{
			CashBoxContext.Update(cb.Id, cb.Name);
        	return RedirectToAction("Index");
        }
		[HttpPost]
		public ActionResult Create(Staff s)
		{
			StaffContext.Create(s.Name);
        	return RedirectToAction("Index");
        }
	}
}