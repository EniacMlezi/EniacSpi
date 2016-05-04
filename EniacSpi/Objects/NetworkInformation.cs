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
    }
}