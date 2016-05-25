using EniacSpi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EniacSpi.Objects
{
    public class HostInformation : IHostInformation
    {
        public string MAC { get; set; }
    }
}