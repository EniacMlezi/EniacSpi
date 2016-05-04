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
using System.Net;

namespace EniacSpi.Controllers
{
    [Authorize(Roles = "User")]
    public class HostController : Controller
    {

        public ActionResult Index(string Name)
        {
            var host = HostManager.Current.GetHost(Name);

            if (host == null)
                return RedirectToRoute(new { Action="Index", Controller="Home" });

            IEnumerable<SelectListItem> availableNetworkDropDownList = host.AvailableNetworks.Select(x => new SelectListItem { Text = x.SSID, Value = x.MAC, Selected = x == host.SelectedNetwork });
            IEnumerable<SelectListItem> availableTargetHostDropDownList = host.AvailableTargetHosts.Select(x => new SelectListItem { });

            var model = new HostViewModel
            {             
                Name = host.Name,
                AvailableNetworkDropDownList = availableNetworkDropDownList ?? Enumerable.Empty<SelectListItem>(),
                AvailableTargetHostsDropDownList = availableTargetHostDropDownList ?? Enumerable.Empty<SelectListItem>()
            };

            return View(model);
        }

        public ActionResult NetworkInformation(string Name, string selectedMAC)
        {
            var host = HostManager.Current.GetHost(Name);

            if (host == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var selectedNetwork = host.AvailableNetworks.Where(network => network.MAC == selectedMAC).FirstOrDefault();

            HostNetworkInformation model = new HostNetworkInformation
            {
                SSID = "N/A",
                MAC = "N/A",
                Security = "N/A",
                Signal = 0
            };

            if (selectedNetwork != null)
            {
                model = new HostNetworkInformation
                {
                    SSID = selectedNetwork.SSID,
                    MAC = selectedNetwork.MAC,
                    Security = selectedNetwork.Security,
                    Signal = selectedNetwork.Signal
                };
            }  

            return View(model);
        }

        public ActionResult TargetHostInformation(string Name, string selectedMAC)
        {
            var host = HostManager.Current.GetHost(Name);

            if (host == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var selectedTargetHost = host.AvailableTargetHosts.Where(network => network.MAC == selectedMAC).FirstOrDefault();

            HostTargetHostInformation model = new HostTargetHostInformation
            {
                MAC = "N/A"
            };

            if (selectedTargetHost != null)
            {
                model = new HostTargetHostInformation
                {
                    MAC = selectedTargetHost.MAC
                };
            }

            return View(model);
        }

        public ActionResult List()
        {
            IEnumerable<IHost> hosts = HostManager.Current.GetHosts();
            var model = hosts.Select(x => new HostListViewModel { Address = x.Address, Name = x.Name });
                   
            return View(model);
        }
    }
}