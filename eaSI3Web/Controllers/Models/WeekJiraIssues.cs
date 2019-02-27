using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class WeekJiraIssues
    {
        public DateTime Fecha { get; set; }
        public List<JiraIssues> Issues { get; set; }
    }
}
