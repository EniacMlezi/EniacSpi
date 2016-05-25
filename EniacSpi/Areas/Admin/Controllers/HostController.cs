using EniacSpi.Areas.Admin.Models;
using EniacSpi.Interfaces;
using EniacSpi.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EniacSpi.Areas.Admin.Controllers
{
    public class HostController : Controller
    {
        // GET: Admin/Module
        public ActionResult Index(string Name)
        {
            var host = HostManager.Current.GetHost(Name);


            return View();
        }

        public ActionResult List()
        {
            IEnumerable<IHost> hosts = HostManager.Current.GetHosts();
            IEnumerable<HostListViewModel> model = hosts.Select(x => new HostListViewModel { Address = x.Address, Name = x.Name });
            return View(model);
        }

        public ActionResult Delete(string Name)
        {
            HostManager.Current.Delete(Name);
            return RedirectToAction("List");
        }
    }
}