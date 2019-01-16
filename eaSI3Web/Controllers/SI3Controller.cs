using System;
using System.Collections.Generic;
using System.Linq;
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

            return "";

            //return new List<WeekJiraIssues>() { new WeekJiraIssues() { Fecha = DateTime.Today.ToString("dd/MM/yyyy"), Issues = new List<WeekJiraIssues.JiraIssues>(){ new WeekJiraIssues.JiraIssues() { IssueId = "Hola", Tiempo = 2.5, Titulo = "Tarea 1" }, new WeekJiraIssues.JiraIssues() { IssueId = "Hola", Tiempo = 5, Titulo = "Tarea 2" } } }  };
        }
    }
}