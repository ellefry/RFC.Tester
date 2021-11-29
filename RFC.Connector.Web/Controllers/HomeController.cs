using RFC.Common;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RFC.Connector.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SapConnectionConfig sapConnectionConfig)
        {
            IDestinationConfiguration eCCDestinationConfig = null;
            RfcDestination rfcDestination = null;

            try
            {
                sapConnectionConfig.IPAddress = sapConnectionConfig.IPAddress ?? "";
                sapConnectionConfig.SystemNumber = sapConnectionConfig.SystemNumber ?? "";
                sapConnectionConfig.SystemID = sapConnectionConfig.SystemID ?? "";
                sapConnectionConfig.User = sapConnectionConfig.User ?? "";
                sapConnectionConfig.Password = sapConnectionConfig.Password ?? "";
                sapConnectionConfig.ReposotoryPassword = sapConnectionConfig.ReposotoryPassword ?? "";
                sapConnectionConfig.Client = sapConnectionConfig.Client ?? "";
                sapConnectionConfig.Language = sapConnectionConfig.Language ?? "";
                sapConnectionConfig.PoolSize = sapConnectionConfig.PoolSize ?? "";


                eCCDestinationConfig = new ECCDestinationConfig(sapConnectionConfig);
                RfcDestinationManager.RegisterDestinationConfiguration(eCCDestinationConfig);
                rfcDestination = RfcDestinationManager.GetDestination("mySAPdestination");

                var repo = rfcDestination.Repository;

                RfcSessionManager.EndContext(rfcDestination);
                RfcDestinationManager.UnregisterDestinationConfiguration(eCCDestinationConfig);
                return Content("Success");
            }
            catch (Exception ex)
            {
                if (rfcDestination != null) 
                    RfcSessionManager.EndContext(rfcDestination);
                if (eCCDestinationConfig != null)
                    RfcDestinationManager.UnregisterDestinationConfiguration(eCCDestinationConfig);
                return Content(ex.Message + Environment.NewLine + ex.StackTrace);
            }
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