using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class TableController : Controller
	{
		private DbConx context = new DbConx();

		public ActionResult Index()
		{
			ViewBag.List = null;
			
			if (Request.QueryString["search"] != null)
			{
				string query = Request.QueryString["search"];
				ViewBag.Query = query;
				
				List<List<string>> result;
				
				result = StaffContext.Search(query);
				if (result != null)
				{
					ViewBag.Staff = result;
				}
				result = OperationTypeContext.Search(query);
				if (result != null)
				{
					ViewBag.OperationType = result;
				}
				result = CashBoxContext.Search(query);
				if (result != null)
				{
					ViewBag.CashBox = result;
				}
				result = ContractContext.Search(query);
				if (result != null)
				{
					ViewBag.Contract = result;
				}
				result = LogBookContext.Search(query);
				if (result != null)
				{
					ViewBag.LogBook = result;
				}

				int k = 12;
			}
			return View();
		}

		public ActionResult Staff()
		{
			if (Request.QueryString["delete"] != null  && Request.QueryString["table"] != null)
			{
				context.DeleteById(
					Request.QueryString["table"],
					Int32.Parse(Request.QueryString["delete"])
				);
			}

			if (Request.QueryString["sort"] != null)
			{
				var allStaff = StaffContext.GetAllStaff(Request.QueryString["sort"]);
				if (allStaff == null)
				{
					ViewBag.StaffList = StaffContext.GetAllStaff("");
				}
			}
			else
				ViewBag.StaffList = StaffContext.GetAllStaff("");

			return View();
		}
		public ActionResult EditStaff()
		{
			ViewBag.Create = false;
			if (Request.QueryString["create"] == "true")
			{
				ViewBag.Create = true;
				return View();
			}

			if (Request.QueryString["id"] != null)
			{
				ViewBag.Staff = StaffContext.GetStaffById(Int32.Parse(Request.QueryString["id"]));
				return View();	
			}

			return RedirectToAction("Staff");
		}
		[HttpPost]
		public ActionResult EditStaff(Staff s)
		{
			if (s.Id != 0)
			{
				StaffContext.Update(s.Id, s.Name);	
			}
			else
			{
				StaffContext.Create(s.Name);
			}
			return RedirectToAction("Staff");
		}
		
		public ActionResult OperationType()
		{
			if (Request.QueryString["delete"] != null  && Request.QueryString["table"] != null)
			{
				context.DeleteById(
					Request.QueryString["table"],
					Int32.Parse(Request.QueryString["delete"])
				);
			}
			
			if (Request.QueryString["sort"] != null)
				ViewBag.OperationTypeList = OperationTypeContext.GetAllOperationTypes(Request.QueryString["sort"]);
			else
				ViewBag.OperationTypeList = OperationTypeContext.GetAllOperationTypes("");
			
			return View();
		}
		public ActionResult EditOperationType()
		{
			ViewBag.Create = false;
			if (Request.QueryString["create"] == "true")
			{
				ViewBag.Create = true;
				return View();
			}

			if (Request.QueryString["id"] != null)
			{
				ViewBag.OperationType = OperationTypeContext.GetOperationTypeById(Int32.Parse(Request.QueryString["id"])); 
				return View();
			}
			
			return RedirectToAction("OperationType");
		}
		[HttpPost]
		public ActionResult EditOperationType(OperationType ot)
		{
			if (ot.Id != 0)
			{
				OperationTypeContext.Update(ot.Id, ot.Type);
			}
			else
			{
				OperationTypeContext.Create(ot.Type);
			}
			return RedirectToAction("OperationType");
		}

		public ActionResult Contract()
		{
			if (Request.QueryString["delete"] != null  && Request.QueryString["table"] != null)
			{
				context.DeleteById(
					Request.QueryString["table"],
					Int32.Parse(Request.QueryString["delete"])
				);
			}
			
			ViewBag.ContractsList = ContractContext.GetAllContracts();
			return View();
		}
		public ActionResult EditContract()
		{
			ViewBag.Create = false;
			ViewBag.StaffList = StaffContext.GetAllStaff("");
			ViewBag.OperationType = OperationTypeContext.GetAllOperationTypes("");
			
			if (Request.QueryString["create"] == "true")
			{
				ViewBag.Create = true;
				return View();
			}

			if (Request.QueryString["id"] != null)
			{
				ViewBag.Contract = ContractContext.GetContractById(Int32.Parse(Request.QueryString["id"])); 
				return View();
			}

			return RedirectToAction("Contract");
		}
		[HttpPost]
		public ActionResult EditContract(Contract c)
		{
			if (c.Id != 0)
			{
				ContractContext.Update(c.Id, c.IdUser, c.IdType, c.CDate, c.Sum);
			}
			else
			{
				ContractContext.Create(c.IdUser, c.IdType, c.CDate, c.Sum);
			}
			return RedirectToAction("Contract");
		}

		public ActionResult Logbook()
		{
			if (Request.QueryString["delete"] != null  && Request.QueryString["table"] != null)
			{
				context.DeleteById(
					Request.QueryString["table"],
					Int32.Parse(Request.QueryString["delete"])
				);
			}
			
			ViewBag.LogBookList = LogBookContext.GetAllLogBooks();
			return View();
		}
		public ActionResult EditLogBook()
		{
			ViewBag.Create = false;
			ViewBag.CashBoxList = CashBoxContext.GetAllCashBox();
			ViewBag.ContractList = ContractContext.GetAllContracts();
			
			if (Request.QueryString["create"] == "true")
			{
				ViewBag.Create = true;
				return View();
			}

			if (Request.QueryString["id"] != null)
			{
				ViewBag.LogBook = LogBookContext.GetLogBookById(Int32.Parse(Request.QueryString["id"]));
				return View();
			}

			return RedirectToAction("Logbook");
		}
		[HttpPost]
		public ActionResult EditLogBook(LogBook lb)
		{
			if (lb.Id != 0)
			{
				LogBookContext.Update(lb.Id, lb.IdCashBox, lb.IdContract, lb.LDate);
			}
			else
			{
				LogBookContext.Create(lb.IdCashBox, lb.IdContract, lb.LDate);
			}
			return RedirectToAction("Logbook");
		}
		
		public ActionResult Cashbox()
		{
			if (Request.QueryString["delete"] != null  && Request.QueryString["table"] != null)
			{
				context.DeleteById(
					Request.QueryString["table"],
					Int32.Parse(Request.QueryString["delete"])
				);
			}
			
			ViewBag.CashBoxList = CashBoxContext.GetAllCashBox();
			return View();
		}
		public ActionResult EditCashbox()
		{
			ViewBag.Create = false;
			if (Request.QueryString["create"] == "true")
			{
				ViewBag.Create = true;
				return View();
			}

			if (Request.QueryString["id"] != null)
			{
				ViewBag.CashBox = CashBoxContext.GetCashBoxById(Int32.Parse(Request.QueryString["id"]));
				return View();
			}

			return RedirectToAction("Cashbox");
		}
        [HttpPost]
        public ActionResult EditCashbox(CashBox cb)
		{
			if (cb.Id != 0)
			{
				CashBoxContext.Update(cb.Id, cb.Name);
			}
			else
			{
				CashBoxContext.Create(cb.Name);
			}
			return RedirectToAction("Cashbox");
		}
	}
}