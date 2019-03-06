using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class Milestones
    {
        public string ProjectCode { get; set; }
        public List<SI3.Projects.Milestone> Milestone {get; set;}
    }
}
