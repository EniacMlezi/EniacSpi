using Dropbox.Api;
using Dropbox.Api.Files;
using EniacSpi.Models;
using EniacSpi.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EniacSpi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();
            ViewBag.Logs = getLogs();
            return View();
        }

        [HttpGet]
        public ActionResult AddNotes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNote(NotesViewModel model)
        {
            string[] tempArray = model.Content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Note note = new Note
            {
                Entered = DateTime.Now,
                MadeBy = User.Identity.Name,
                Title = model.Title,
                Values = tempArray
            };
            dynamic tempX = JObject.FromObject(note);
            JArray jsonVal = new JArray();
            jsonVal.Add(tempX);
            if (new FileInfo("c:\\Log.json").Length != 250)
            {
                JArray jsonTemp = JArray.Parse(System.IO.File.ReadAllText("C:\\Log.json"));
                foreach (JObject j in jsonTemp)
                {
                    jsonVal.Add(j);
                }
            }

            System.IO.File.WriteAllText("C:\\Log.json", jsonVal.ToString());
            return RedirectToAction("Index", "Home");
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