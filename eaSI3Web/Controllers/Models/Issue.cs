using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class Issue
    {
        public string product { get; set; }
        public string component { get; set; }
        public string module = "0";
        public string title { get; set; }
        public string cause { get; set; }
        public string user { get; set; }
        public string type { get; set; }
        public string tipo { get; set; }
        public string phase { get; set; }
        public string level { get; set; }
        public string priority { get; set; }
    }
}
