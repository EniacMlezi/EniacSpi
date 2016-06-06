using EniacSpi.Interfaces;
using oclHashcatNet.Extensions;
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
        
        public IEnumerable<SelectListItem> AvailableTargetHostsDropDownList { get; set; }
        public IHostInformation SelectedTargetHost { get; set; }
    }

    public class HostNetworkInformationModel
    {
        public string SSID { get; set; }
        public string MAC { get; set; }
        public string Security { get; set; }
        public int Signal { get; set; }
    }

    public class HostNetworkInfiltrationModel
    {
        public bool IsCracking { get; set; }
        public int CrackProgressStatus { get; set; }
        public int CrackProgressEnd { get; set; }
        public string Password { get; set; }
    }

    public class HostNetworkInfiltrationStatusModel
    {
        public string Condition { get; set; }

        public IEnumerable<GPUStatus> GPUs { get; set; }
    }


    public class HostTargetHostInformationModel
    {
        public string MAC { get; set; }
    }
}