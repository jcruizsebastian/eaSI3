using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class WeekJiraIssuesResponse
    {
        public IEnumerable<WeekJiraIssues> WeekJiraIssues { get; set; }
        public bool NotOk { get; set; }
        public string Message { get; set; }
    }
}
