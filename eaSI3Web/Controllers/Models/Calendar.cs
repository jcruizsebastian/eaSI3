using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class Calendar
    {   public string version { get; set; }
        public List<CalendarWeeks> Weeks { get; set; }
    }
}
