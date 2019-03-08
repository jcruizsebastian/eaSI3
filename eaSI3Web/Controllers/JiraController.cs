using eaSI3Web.Controllers.UsageStatistics;
using eaSI3Web.Models;
using IssueConveter.Model;
using JiraConnector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using eaSI3Web.Controllers.Models;
using System.Text.RegularExpressions;

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

        static Models.Calendar calendar = new Models.Calendar();

        [HttpGet("[action]")]
        public ActionResult<Models.Calendar> Weeks()
        {
            var version = GetType().Assembly.GetName().Version.ToString();
            calendar.version = version;
            calendar.Weeks = new List<CalendarWeeks>();
            int weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            int intDay = 1;
            int intMonth = 1;

            for (int i = 0; i < weekOfYear; i++)
            {

                DateTime day = new DateTime(DateTime.Now.Year, intMonth, intDay);
                int aSumar = 6;

                //Este if hay que revisarlo , no sirve para todos los años, solo para el actual
                //i == 0 es lo mismo que comprobar que es la primera semana.
                if (i == 0) { aSumar = 5; }

                //Semana entre dos meses
                if (intDay + aSumar > CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(DateTime.Now.Year, day.Month))
                {
                    intDay = (intDay + aSumar) - CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(DateTime.Now.Year, day.Month);
                    aSumar = 0;
                    intMonth += 1;
                }

                string description = day.Day + "/" + day.Month + "/" + DateTime.Now.Year + " to " + (intDay + aSumar) + "/" + intMonth + "/" + DateTime.Now.Year;


                calendar.Weeks.Add(new CalendarWeeks()
                {
                    numberWeek = i + 1,
                    description = description,
                    startOfWeek = new DateTime(DateTime.Now.Year, day.Month, day.Day),
                    endOfWeek = new DateTime(DateTime.Now.Year, intMonth, (intDay + aSumar))
                });
                intDay += aSumar + 1;
            }

            return calendar;
        }

        [HttpGet("[action]")]
        public ActionResult<WeekJiraIssuesResponse> Worklog(string username, string password, string selectedWeek)
        {
            DateTime startOfWeek = DateTime.Now;
            DateTime endOfWeek = DateTime.Now;
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
                _logger.LogError("Usuario : " + username + " Semana Elegida : " + selectedWeek + "Error : " + e.Message);
                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            WeekJiraIssuesResponse weekJiraIssuesList = new WeekJiraIssuesResponse();
            weekJiraIssuesList.WeekJiraIssues = Convert(currentWorklog);
            _logger.LogInformation("Worklog devuelto satisfactoriamente a " + username);
            return weekJiraIssuesList;
        }

        [HttpGet("[action]")]
        public ActionResult<IssueConveter.Model.Issue> Issue(string username, string password, string jiraKey)
        {
            var jiraIssue = new IssueConveter.Model.Issue();
            BdStatistics bdStatistics = new BdStatistics(_context);

            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
                jiraIssue = jiraWorkLogService.GetIssue(jiraKey);
            }
            catch (Exception e)
            {

                bdStatistics.AddIssueCreation(username, jiraKey, jiraIssue.si3ID, 1, e.Message);
                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            if (jiraIssue.si3ID != null)
            {
                bdStatistics.AddIssueCreation(username, jiraKey, jiraIssue.si3ID, 2, "Tarea ya vinculada");
            }
            return jiraIssue;

        }
        [HttpGet("[action]")]
        public ActionResult ValidateLogin(string username, string password)
        {
            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
            }
            catch (Exception e)
            {
                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);

            }

            return Ok();
        }
        [HttpGet("[action]")]
        public ActionResult<string> updateIssueSi3Project(string username, string password, string codeProject, string codeMilestone, string jiraKey)
        {
            BdStatistics bdStatistics = new BdStatistics(_context);

            Regex regex = new Regex(@"^(H\.[0-9]{1,4}).+$");
            Match match = regex.Match(codeMilestone);
            if (match.Success)
            {
                codeMilestone = match.Groups[1].Value;
                codeMilestone = codeMilestone.Replace(".", "-");
            }

            Regex regex2 = new Regex(@"^(O)([0-9]+)$");
            Match match2 = regex2.Match(codeProject);
            if (match2.Success)
            {
                codeProject = match2.Groups[1].Value + "-" + match2.Groups[2].Value;
            }

            var key = codeProject + "," + codeMilestone;

            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
                string body = JsonConvert.SerializeObject(new { fields = new { customfield_10300 = key } });
                jiraWorkLogService.UpdateIssue(jiraKey, body);
            }
            catch (Exception e)
            {
                bdStatistics.AddIssueCreation(username, jiraKey, key, 1, e.Message);

                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            bdStatistics.AddIssueCreation(username, jiraKey, key, 0, "Tarea vinculada correctamente");
            return key;
        }
        [HttpGet("[action]")]
        public ActionResult UpdateIssueSi3CustomField(string username, string password, [RegularExpression("[0-9]+")]string issueKey, [RegularExpression("\\w+-[0-9]+")]string jirakey)
        {
            BdStatistics bdStatistics = new BdStatistics(_context);

            if (!ModelState.IsValid)
                return StatusCode(400, "");

            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(username, password);
                string body = JsonConvert.SerializeObject(new { fields = new { customfield_10300 = issueKey } });
                jiraWorkLogService.UpdateIssue(jirakey, body);
            }
            catch (Exception e)
            {
                bdStatistics.AddIssueCreation(username, jirakey, issueKey, 1, e.Message);

                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            bdStatistics.AddIssueCreation(username, jirakey, issueKey, 0, "Tarea vinculada correctamente");
            return Ok();
        }

        public static IEnumerable<WeekJiraIssues> Convert(List<WorkLog> worklog)
        {
            List<WeekJiraIssues> weekJiraIssues = new List<WeekJiraIssues>();

            foreach (var workDate in worklog.GroupBy(x => x.RecordDate.Date))
            {
                WeekJiraIssues dateIssues = new WeekJiraIssues();
                dateIssues.Fecha = workDate.First().RecordDate.Date;

                dateIssues.Issues = new List<JiraIssues>();

                foreach (var work in workDate)
                {
                    var issue = dateIssues.Issues.SingleOrDefault(x => x.IssueKey == work.Key);
                    if (issue != null)
                        issue.Tiempo += (work.TimeSpentSeconds / 3600.0);
                    else
                        dateIssues.Issues.Add(new JiraIssues() { Titulo = work.Summary + " - " + work.Comment, IssueKey = work.Key, IssueCode = work.IssueId, Tiempo = (work.TimeSpentSeconds / 3600.0), IssueSI3Code = work.si3ID });
                }
                weekJiraIssues.Add(dateIssues);
            }

            return weekJiraIssues.OrderBy(d => d.Fecha);
        }

    }
}
