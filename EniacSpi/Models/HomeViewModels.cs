using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EniacSpi.Models
{
    public class NotesViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}