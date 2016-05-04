using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EniacSpi.Models
{ 
    public class HostListViewModel
    {
        public string Name { get; set; }   
        public string Address { get; set; }
    }

    public class HostViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        
        public IEnumerable<SelectListItem> AvailableNetworkDropDownList { get; set; }
        public INetworkInformation SelectedNetwork { get; set; }

        public bool IsCracked { get; set; }
        public bool IsCracking { get; set; }
        public int CrackProgress { get; set; }

        public IEnumerable<SelectListItem> AvailableHostsDropDownList { get; set; }  
        public IEnumerable<SelectListItem> AvailableTargetHostsDropDownList { get; set; }
        public int SelectedHost { get; set; }
    }

    public class HostNetworkInformation
    {
        public string SSID { get; set; }
        public string MAC { get; set; }
        public string Security { get; set; }
        public int Signal { get; set; }
    }

    public class HostTargetHostInformation
    {
        public string MAC { get; set; }
    }
}