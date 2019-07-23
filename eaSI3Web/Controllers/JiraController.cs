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
using System.Configuration;
using eaSI3Web.Configs;
using Microsoft.Extensions.Options;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class JiraController : Controller
    {
        public IOptions<Data> data;
        private readonly StatisticsContext _context;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public JiraController( StatisticsContext context, IOptions<Data> data)
        {
            this.data = data;
            _context = context;   
        }

        static Models.Calendar calendar = new Models.Calendar();
        
        [HttpGet("[action]")]
        public ActionResult<Models.Calendar> Weeks()
        {

            var version = GetType().Assembly.GetName().Version.ToString();
            calendar.version = version;
            calendar.Weeks = new List<CalendarWeeks>();
            DateTime lastDayOfYear = new DateTime(DateTime.Now.Year, 12, 31);
            int weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(lastDayOfYear, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

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

                if (intDay + aSumar == CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(DateTime.Now.Year, day.Month))
                {
                    intDay =  CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(DateTime.Now.Year, day.Month);
                    aSumar = 0;
                }

                

                var year = DateTime.Now.Year ;
                if (intMonth == 13)
                {
                    year = DateTime.Now.Year + 1;
                    intMonth = 1;
                }

                string description = day.Day + "/" + day.Month + "/" + DateTime.Now.Year + " to " + (intDay + aSumar) + "/" + intMonth + "/" + year;

                calendar.Weeks.Add(new CalendarWeeks()
                {
                    numberWeek = i + 1,
                    description = description,
                    startOfWeek = new DateTime(DateTime.Now.Year, day.Month, day.Day),
                    endOfWeek = new DateTime(year, intMonth, (intDay + aSumar))
                });

                intDay += aSumar + 1;
                if (intDay > CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(DateTime.Now.Year, day.Month))
                {
                    intDay = 1;
                    intMonth += 1;
                }
                
            }

            return calendar;
        }
        

        [HttpGet("[action]")]
        public ActionResult<WeekJiraIssuesResponse> Worklog(string selectedWeek)
        {
            DateTime startOfWeek = DateTime.Now;
            DateTime endOfWeek = DateTime.Now;

            foreach (var week in calendar.Weeks)
            {
                if (int.Parse(selectedWeek) == week.numberWeek)
                {
                    startOfWeek = week.startOfWeek.AddDays(-1);
                    endOfWeek = week.endOfWeek;
                }
            }

            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));

            JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(user.JiraUserName, bdStatistics.DecryptJiraPassword(user.JiraUserName), data.Value.Jira_Host_URL);
            var currentWorklog = new List<WorkLog>();
            try
            {
                currentWorklog = jiraWorkLogService.GetWorklog(startOfWeek, endOfWeek, user.JiraUserName);
            }
            catch (Exception e)
            {
                logger.Error("Usuario : " + user.JiraUserName + " Semana Elegida : " + selectedWeek + "Error : " + e.Message);
                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            WeekJiraIssuesResponse weekJiraIssuesList = new WeekJiraIssuesResponse();
            weekJiraIssuesList.WeekJiraIssues = Convert(currentWorklog);
            return weekJiraIssuesList;
        }

        [HttpGet("[action]")]
        public ActionResult<IssueConveter.Model.Issue> Issue(string jiraKey)
        {
            var jiraIssue = new IssueConveter.Model.Issue();
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));

            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(user.JiraUserName, bdStatistics.DecryptJiraPassword(user.JiraUserName), data.Value.Jira_Host_URL);
                jiraIssue = jiraWorkLogService.GetIssue(jiraKey);
            }
            catch (Exception e)
            {
                logger.Error("Username: " + user.JiraUserName + " ,Issue: " + jiraKey + " ,Error: " + e.Message);
                bdStatistics.AddIssueCreation(user.JiraUserName, jiraKey, jiraIssue.si3ID, 1, e.Message);
                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            if (jiraIssue.si3ID != null)
            {
                bdStatistics.AddIssueCreation(user.JiraUserName, jiraKey, jiraIssue.si3ID, 2, "Tarea ya vinculada");
            }
            return jiraIssue;

        }
        [HttpPost("[action]")]
        public ActionResult ValidateLogin() {
            var idUser = "";

            if (Request.Cookies.ContainsKey("userId"))
            {
                var cookie = Request.Cookies.First(x => x.Key == "userId");
                idUser = cookie.Value;
            }
            else
            {
                return StatusCode(400,"Por favor, inicia sesión de nuevo");
            }

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));

            if (user.Password_Encrypted == null)
            {
                return StatusCode(400, "Por favor, inicia sesión de nuevo");
            }


            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(user.JiraUserName, bdStatistics.DecryptJiraPassword(user.JiraUserName), data.Value.Jira_Host_URL);
            }
            catch (Exception e)
            {
                logger.Error("Username: " + user.JiraUserName + " ,Error: " + e.Message);
                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);
            }

            return Ok();
        }
        [HttpPost("[action]")]
        public ActionResult Login([FromBody] BodyData bodyData)
        {
            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(bodyData.usernameJira, bodyData.passwordJira, data.Value.Jira_Host_URL);
            }
            catch (Exception e)
            {
                logger.Error("Username: " + bodyData.usernameJira + " ,Error: " + e.Message);
                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);

            }

            return Ok();
        }
        [HttpGet("[action]")]
        public ActionResult<string> updateIssueSi3Project(string codeProject, string codeMilestone, string jiraKey, string idSi3)
        {
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));

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
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(user.JiraUserName, bdStatistics.DecryptJiraPassword(user.JiraUserName), data.Value.Jira_Host_URL);

                if (idSi3 != "null")
                {
                    key = idSi3 + key;
                }

                string body = JsonConvert.SerializeObject(new { fields = new { customfield_10300 = key } });
                jiraWorkLogService.UpdateIssue(jiraKey, body);
            }
            catch (Exception e)
            {
                logger.Error("Username: " + user.JiraUserName + " ,Error: " + e.Message);
                bdStatistics.AddIssueCreation(user.JiraUserName, jiraKey, key, 1, e.Message);

                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            bdStatistics.AddIssueCreation(user.JiraUserName, jiraKey, key, 0, "Tarea vinculada correctamente");
            return key;
        }
        [HttpGet("[action]")]
        public ActionResult UpdateIssueSi3CustomField([RegularExpression("[0-9]+")]string issueKey, [RegularExpression("\\w+-[0-9]+")]string jirakey)
        {
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));

            if (!ModelState.IsValid)
                return StatusCode(400, "");

            try
            {
                JiraWorkLogService jiraWorkLogService = new JiraWorkLogService(user.JiraUserName, bdStatistics.DecryptJiraPassword(user.JiraUserName), data.Value.Jira_Host_URL);
                string body = JsonConvert.SerializeObject(new { fields = new { customfield_10300 = issueKey } });
                jiraWorkLogService.UpdateIssue(jirakey, body);
            }
            catch (Exception e)
            {
                logger.Error("Username: " + user.JiraUserName + " ,Error: " + e.Message);
                bdStatistics.AddIssueCreation(user.JiraUserName, jirakey, issueKey, 1, e.Message);

                if (e is InvalidCredentialException || e is UnauthorizedAccessException || e is InvalidOperationException)
                    return StatusCode(401, e.Message);

                return StatusCode(500, e.Message);
            }

            bdStatistics.AddIssueCreation(user.JiraUserName, jirakey, issueKey, 0, "Tarea vinculada correctamente");
            return Ok();
        }

        private static IEnumerable<WeekJiraIssues> Convert(List<WorkLog> worklog)
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
                        dateIssues.Issues.Add(new JiraIssues() { Titulo = work.Summary + " - " + work.Comment, IssueKey = work.Key, IssueCode = work.IssueId, Tiempo = (work.TimeSpentSeconds / 3600.0), IssueSI3Code = work.si3ID, Tipo = work.Type });
                }
                weekJiraIssues.Add(dateIssues);
            }

            return weekJiraIssues.OrderBy(d => d.Fecha);
        }

    }
}
