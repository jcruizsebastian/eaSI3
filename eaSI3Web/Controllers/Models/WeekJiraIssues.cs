using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class WeekJiraIssues
    {
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public List<JiraIssues> Issues { get; set; }
    }
}
