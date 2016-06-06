using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EniacSpi.Objects
{
    public class Note
    {
        //the title of the note
        public string Title { get; set; }
        //the user who added the note
        public string MadeBy { get; set; }
        //the time the note was added
        public DateTime Entered { get; set; }
        //the content for the note
        public string[] Values { get; set; }
    }
}