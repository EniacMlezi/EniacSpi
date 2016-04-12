using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EniacSpi.Models
{
    public class WidgetListViewModel
    {
        public List<WidgetViewModel> Widgets { get; set; }
    }

    public class WidgetViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        
        public string PositionX { get; set; }
        public string PositionY { get; set; }
    }
}