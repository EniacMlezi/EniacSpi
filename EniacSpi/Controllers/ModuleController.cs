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
    public class ModuleController : Controller
    {
        // GET: Module
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            IEnumerable<IModule> modules = ModuleManager.Current.GetModules();
            var model = modules.Select(x => new ModuleListViewModel { Address = x.Address, Name = x.Name });
                   
            return View(model);
        }
    }
}