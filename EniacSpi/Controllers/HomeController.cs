using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EniacSpi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();
            ViewBag.Logs = getLogs();
            return View();
        }

        public dynamic getLogs()
        {
            string jsonString;
            using (StreamReader stream = new StreamReader(@"c:\Test.json"))
            {
                jsonString = stream.ReadToEnd();
            }                
            JArray jsonVal = JArray.Parse(jsonString) as JArray;
            dynamic Logs = jsonVal;
            return Logs;
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