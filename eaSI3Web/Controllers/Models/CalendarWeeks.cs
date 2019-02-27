using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class CalendarWeeks
    {
        public int numberWeek { get; set; }
        public string description { get; set; }
        public DateTime startOfWeek { get; set; }
        public DateTime endOfWeek { get; set; }
    }
}
