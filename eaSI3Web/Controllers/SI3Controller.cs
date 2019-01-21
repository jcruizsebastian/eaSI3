using System;
using System.Collections.Generic;
using System.Linq;
using SI3Connector;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static eaSI3Web.Controllers.JiraController;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class SI3Controller : Controller
    {
        [HttpPost("[action]")]
        public string Register([FromQuery]string username, [FromQuery]string password, [FromBody]IEnumerable<WeekJiraIssues> model)
        {
            SI3Service SI3Service = new SI3Service(username, password);

            //foreach(var dateIssues in model)
            //{
            //    foreach (var issue in dateIssues.Issues)
            //    {
            //        SI3Service.AddWorklog(issue.IssueId, dateIssues.Fecha, issue.Tiempo);
            //    }
            //}

            return "";
        }
    }
}