using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EniacSpi.Models;
using System.ComponentModel.Composition;
using System.Collections;
using EniacSpi.Interfaces;
using EniacSpi.Objects;

namespace EniacSpi.Controllers
{
    [Authorize(Roles = "User")]
    public class HostController : Controller
    {
        // GET: Module
        public ActionResult Index(string Name)
        {

            return View();
        }

        public ActionResult List()
        {
            IEnumerable<IHost> hosts = HostManager.Current.GetModules();
            var model = hosts.Select(x => new HostListViewModel { Address = x.Address, Name = x.Name });
                   
            return View(model);
        }
    }
}