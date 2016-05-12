using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EniacSpi.Objects
{
    public class NetworkInformation : INetworkInformation
    {
        public string SSID { get; set; }
        public string MAC { get; set; }
        public string Security { get; set; }
        public int Signal { get; set; }
       
        public bool IsCracking { get; set; }
        public int CrackProgressStatus { get; set; }
        public int CrackProgressEnd { get; set; }
        public string Password { get; set; }
    }
}