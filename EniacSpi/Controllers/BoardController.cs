using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EniacSpi.Models;
using System.Web.Script.Serialization;
using EniacSpi.Objects;

namespace EniacSpi.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        // GET: Board
        public ActionResult Index()
        {   
            var Modules = ModuleManager.Current.GetModules();

            var widgets = Modules.Select(x => new WidgetViewModel
            {
                Name = x.Name,
                Address = x.Address,
                PositionX = (Request.Cookies["Dashboard"] == null) ? null : Request.Cookies["Dashboard"][x.Name].Split(',')[0],
                PositionY = (Request.Cookies["Dashboard"] == null) ? null : Request.Cookies["Dashboard"][x.Name].Split(',')[1]
            }).ToList();
            

            return View(new WidgetListViewModel { Widgets = widgets });
        }

        private struct widget
        {
            public string Name { get; set; }
            public string x { get; set; }
            public string y { get; set; }
        }
        public void UpdateCookie(string[] widgets)
        {
            HttpCookie DashboardCookie = new HttpCookie("Dashboard");
            foreach (string widget in widgets)
            {
                widget objWidget = new JavaScriptSerializer().Deserialize<widget>(widget);
                DashboardCookie[objWidget.Name] = objWidget.x + "," + objWidget.y;
            }
            DashboardCookie.Expires = DateTime.Now.AddYears(10);
            Response.Cookies.Add(DashboardCookie);  
            return;
        }
    }
}