using System;
using System.Linq;
using System.Collections.Generic;
using JiraConnector;
using Microsoft.AspNetCore.Mvc;
using IssueConveter.Model;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class JiraController : Controller
    {
        [HttpGet("[action]")]
        public IEnumerable<WeekJiraIssues> Worklog(string username, string password)
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-1 * ((int)(DateTime.Today.DayOfWeek + 6) % 7)).AddDays(-1);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
            var currentWorklog = jiraWorkLogService.GetWorklog(startOfWeek, endOfWeek, username);

            return Convert(currentWorklog);

            //return new List<WeekJiraIssues>() { new WeekJiraIssues() { Fecha = DateTime.Today.ToString("dd/MM/yyyy"), Issues = new List<WeekJiraIssues.JiraIssues>(){ new WeekJiraIssues.JiraIssues() { IssueId = "Hola", Tiempo = 2.5, Titulo = "Tarea 1" }, new WeekJiraIssues.JiraIssues() { IssueId = "Hola", Tiempo = 5, Titulo = "Tarea 2" } } }  };
        }

        public static IEnumerable<WeekJiraIssues> Convert(List<WorkLog> worklog)
        {
            List<WeekJiraIssues> weekJiraIssues = new List<WeekJiraIssues>();

            foreach (var workDate in worklog.GroupBy(x => x.RecordDate.Date))
            {
                WeekJiraIssues dateIssues = new WeekJiraIssues();
                dateIssues.Fecha = workDate.First().RecordDate;//.ToString("dd/MM/yyyy");

                dateIssues.Issues = new List<WeekJiraIssues.JiraIssues>();

                foreach(var work in workDate)
                {
                    dateIssues.Issues.Add(new WeekJiraIssues.JiraIssues() { Titulo = work.Summary + " - " + work.Comment, IssueKey = work.Key, IssueCode = work.IssueId, Tiempo = work.TimeSpent, IssueSI3Code =  work.si3ID });
                }
                weekJiraIssues.Add(dateIssues);
            }

            return weekJiraIssues;
        }

        public class WeekJiraIssues
        {
            public DateTime Fecha { get; set; }
            public List<JiraIssues> Issues { get; set; }

            public class JiraIssues
            {
                public string IssueSI3Code { get; set; }
                public string IssueCode { get; set; }
                public string IssueKey { get; set; }
                public string Titulo { get; set; }
                public string Tiempo { get; set; }
            }
        }
    }
}
