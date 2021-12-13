using Sap.Conn.Service.DataStorage;
using Sap.Conn.Service.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sap.Conn.Service.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new SapConnectorContext())
            {
                //db.Set<ProcessRequest>().Add(new ProcessRequest { FunctionType = 0});
                //db.SaveChanges();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}