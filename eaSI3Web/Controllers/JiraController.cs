using eaSI3Web.Models;
using IssueConveter.Model;
using JiraConnector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class JiraController : Controller
    {
        private readonly StatisticsContext _context;

        private readonly ILogger<JiraController> _logger;
        public JiraController(ILogger<JiraController> logger, StatisticsContext context)
        {
            _logger = logger;
            _context = context;
        }

        static Calendar calendar = new Calendar();

        [HttpGet("[action]")]
        public Calendar Weeks() {
           
            calendar.Weeks = new List<Calendar.CalendarWeeks>();
            int weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            int intDay = 1;
            int intMonth = 1;

            for (int i = 0; i < weekOfYear; i++) {
                
                DateTime day = new DateTime(DateTime.Now.Year,intMonth,intDay);
                int aSumar = 6;

                //Este if hay que revisarlo , no sirve para todos los años, solo para el actual
                //i == 0 es lo mismo que comprobar que es la primera semana.
                if (i==0) { aSumar = 5;  }

                //Semana entre dos meses
                if (intDay+aSumar > CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(DateTime.Now.Year, day.Month)) {
                    intDay = (intDay + aSumar) - CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(DateTime.Now.Year, day.Month);
                    aSumar = 0;
                    intMonth += 1;
                }

                string description = day.Day + "/" + day.Month + "/" + DateTime.Now.Year + " to " + (intDay+aSumar) + "/" + intMonth + "/" + DateTime.Now.Year ;


                calendar.Weeks.Add(new Calendar.CalendarWeeks() {
                    numberWeek = i + 1,
                    description = description,
                    startOfWeek = new DateTime(DateTime.Now.Year, day.Month, day.Day),
                    endOfWeek = new DateTime(DateTime.Now.Year, intMonth,(intDay+aSumar))
                });
                intDay += aSumar+1;
            }

            
            return calendar;
        }

        [HttpGet("[action]")]
        public WeekJiraIssuesResponse Worklog(string username, string password, string selectedWeek)   
        {
            DateTime startOfWeek = DateTime.Now;
            DateTime endOfWeek= DateTime.Now;
            _logger.LogInformation("Usuario " + username + " => clic en el botón de Enviar Jira, Semana elegida : " + selectedWeek);

            foreach (var week in calendar.Weeks)
            {
                if (int.Parse(selectedWeek) == week.numberWeek)
                {
                    startOfWeek = week.startOfWeek.AddDays(-1);
                    endOfWeek = week.endOfWeek;
                }
            }
            
            JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
            var currentWorklog = new List<WorkLog>();
            try
            {
                currentWorklog = jiraWorkLogService.GetWorklog(startOfWeek, endOfWeek, username);
            }
            catch (Exception e)
            {
                List<WeekJiraIssues> weekJiraIssues = new List<WeekJiraIssues>();
                WeekJiraIssuesResponse weekJiraIssuesListError = new WeekJiraIssuesResponse();
                weekJiraIssuesListError.WeekJiraIssues = weekJiraIssues;
                weekJiraIssuesListError.NotOk = true;
                weekJiraIssuesListError.Message = e.Message;
                _logger.LogError("Usuario : " + username + " Semana Elegida : " + selectedWeek + "Error : " +e.Message);
                return weekJiraIssuesListError;
            }
            
            WeekJiraIssuesResponse weekJiraIssuesList = new WeekJiraIssuesResponse();
            weekJiraIssuesList.WeekJiraIssues = Convert(currentWorklog);
            weekJiraIssuesList.NotOk = false;
            weekJiraIssuesList.Message = "Todo bien, todo correcto";
            _logger.LogInformation("Worklog devuelto satisfactoriamente a " + username);
            return weekJiraIssuesList;
        }

        [HttpGet("[action]")]
        public Issue Issue(string username, string password, string jiraKey)
        {
            JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);

            var jiraIssue = jiraWorkLogService.GetIssue(jiraKey);

            return jiraIssue;

        }
        [HttpGet("[action]")]
        public string ValidateLogin(string username, string password)
        {
            var user = from u in _context.Users where u.JiraUserName.Equals(username) select u;

            if (!user.Any())
                _context.Add(new User() { JiraUserName = username });

            user = from u in _context.Users where u.JiraUserName.Equals(username) select u;

            _context.Add(new Login() { User = (User)user, ConnectionDate = DateTime.Now });

            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return string.Empty;
        }
        [HttpGet("[action]")]
        public void UpdateIssueSi3CustomField(string username, string password, string issueKey, string jirakey) {

            JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
            string body = JsonConvert.SerializeObject(new { fields = new { customfield_10300 = issueKey } });
            jiraWorkLogService.UpdateIssue(jirakey, body);

        }
        public static IEnumerable<WeekJiraIssues> Convert(List<WorkLog> worklog)
        {
            List<WeekJiraIssues> weekJiraIssues = new List<WeekJiraIssues>();

            foreach (var workDate in worklog.GroupBy(x => x.RecordDate.Date))
            {
                WeekJiraIssues dateIssues = new WeekJiraIssues();
                dateIssues.Fecha = workDate.First().RecordDate.Date;

                dateIssues.Issues = new List<WeekJiraIssues.JiraIssues>();

                foreach(var work in workDate)
                {
                    var issue = dateIssues.Issues.SingleOrDefault(x => x.IssueKey == work.Key);
                    if (issue != null)
                        issue.Tiempo += (work.TimeSpentSeconds / 3600.0);
                    else
                        dateIssues.Issues.Add(new WeekJiraIssues.JiraIssues() { Titulo = work.Summary + " - " + work.Comment, IssueKey = work.Key, IssueCode = work.IssueId, Tiempo = (work.TimeSpentSeconds / 3600.0), IssueSI3Code = work.si3ID });
                }
                weekJiraIssues.Add(dateIssues);
            }

            return weekJiraIssues.OrderBy(d => d.Fecha);
        }

        public class WeekJiraIssuesResponse {
            public IEnumerable<WeekJiraIssues> WeekJiraIssues { get; set; }
            public bool NotOk { get; set; }
            public string Message { get; set; }

        }

        public class Calendar {
            public List<CalendarWeeks> Weeks { get; set; }

            public class CalendarWeeks {

                public int numberWeek { get; set; }
                public string description { get; set; }
                public DateTime startOfWeek { get; set; }
                public DateTime endOfWeek { get; set; }
            }
        }
      
        public class WeekJiraIssues
        {
            public DateTime Fecha { get; set; }
            public List<JiraIssues> Issues { get; set; }

            public class JiraIssues : ICloneable
            {
                public string IssueSI3Code { get; set; }
                public string IssueCode { get; set; }
                public string IssueKey { get; set; }
                public string Titulo { get; set; }
                public double Tiempo { get; set; }

                public object Clone()
                {
                    return (JiraIssues)this.MemberwiseClone();
                }
            }
        }
    }
}
