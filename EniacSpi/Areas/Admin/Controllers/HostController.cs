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
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            IEnumerable<IHost> hosts = HostManager.Current.GetHosts();
            IEnumerable<HostListViewModel> model = hosts.Select(x => new HostListViewModel { Address = x.EndPoint.Address.ToString(), Name = x.Name });
            ViewBag.State = HostFinder.IsListening ? "Stop" : "Start";

            return View(model);
        }

        public void startListening()
        {
            HostFinder.StartListening();
        }

        public void StopListening()
        {
            HostFinder.StopListening();
        }

        public ActionResult Delete(string Name)
        {
            HostManager.Current.Delete(Name);
            return RedirectToAction("List");
        }
    }
}