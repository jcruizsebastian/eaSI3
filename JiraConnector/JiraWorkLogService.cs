using System;
using System.Collections.Generic;
using System.Linq;
using Jira;
using RestSharp;

namespace JiraConnector
{
    public class JiraWorkLogService
    {
        private string _username { get; set; }
        private string _password { get; set; }

        public JiraWorkLogService(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public List<WorkLog> GetCurrentWeekWorkLog()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-1 * ((int)(DateTime.Today.DayOfWeek + 6) % 7)).AddDays(-1);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            string queryStringRequest = string.Format($"rest/api/2/search?jql=issueFunction in workLogged(\"by {_username} after {startOfWeek.ToString("yyyy/MM/dd")} before {endOfWeek.ToString("yyyy/MM/dd")}\")");

            var response = DoJiraRequest<ListIssues>(queryStringRequest);

            List<WorkLog> workLog = new List<WorkLog>();
            if (response.Data == null || response.Data.Issues == null)
                return workLog;

            foreach (var issue in response.Data.Issues)
            {
                workLog.AddRange(GetWorkLog(issue.id));
            }

            workLog.RemoveAll(x => (x.RecordDate < startOfWeek || x.RecordDate > endOfWeek) || x.Author.key != _username);

            foreach (var log in workLog)
            {
                log.si3ID = response.Data.Issues.First(x => x.id == log.IssueId)?.fields?.customfield_10300?.ToString();
                var issue = GetIssue(log.IssueId);
                log.Summary = issue.Summary;
                log.Key = issue.Key;
            }

            workLog.RemoveAll(x => string.IsNullOrEmpty(x.si3ID));

            return workLog;
        }

        public List<WorkLog> GetWorkLog(string key)
        {
            string queryStringRequest = string.Format("rest/api/2/issue/{0}/worklog", key);

            var response = DoJiraRequest<JiraWorkLog>(queryStringRequest);

            if (response == null || response.Data == null)
                throw new Exception("Error with : " + key);

            return ConvertToWorklog(response.Data);
        }

        public Issue GetIssue(string key)
        {
            string queryStringRequest = string.Format("rest/api/2/issue/{0}", key);

            var response = DoJiraRequest<JiraIssue>(queryStringRequest);

            if (response == null || response.Data == null)
                throw new Exception("Error with : " + key);

            return ConvertToJiraIssue(response.Data);
        }

        private Issue ConvertToJiraIssue(JiraIssue data)
        {
            return new Issue() { Key = data.key, Summary= data.fields.summary };
        }

        private List<WorkLog> ConvertToWorklog(JiraWorkLog data)
        {
            var worklogs = new List<WorkLog>();

            foreach (var log in data.worklogs)
            {
                var worklog = new WorkLog() { IssueId = log.issueId, Author = log.author, TimeSpentSeconds = log.timeSpentSeconds, TimeSpent = log.timeSpent, Comment = log.comment, RecordDate = log.started };

                worklogs.Add(worklog);
            }

            return worklogs;
        }

        private IRestResponse<T> DoJiraRequest<T>(string queryStringRequest) where T : new()
        {
            var request = new RestRequest(queryStringRequest, Method.GET);

            request.AddHeader("Authorization", string.Format("Basic {0}", Base64Encode($"{_username}:{_password}")));

            var client = new RestClient("https://jira.openfinance.es/");

            return client.Execute<T>(request);
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
