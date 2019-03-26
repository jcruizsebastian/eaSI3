using IssueConveter;
using IssueConveter.Model;
using Jira;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JiraConnector
{
    public class JiraWorkLogService
    {
        private JiraHttpRequest jiraHttpRequest { get; set; }

        public JiraWorkLogService(string username, string password, string jiraUrl)
        {
            jiraHttpRequest = new JiraHttpRequest(username, password, jiraUrl);

            //Probamos que se hace login correctamente con las credenciales recibidas
            jiraHttpRequest.DoJiraRequest<JiraIssue>(JiraURIRepository.LOGIN(username), Method.GET);
        }

        public void UpdateIssue(string issueKey, string jsonBody)
        {
            var response = jiraHttpRequest.DoJiraRequest<IssuesList>(JiraURIRepository.UPDATE_ISSUE(issueKey), Method.PUT, jsonBody);
        }

        public List<WorkLog> GetWorklog(DateTime startDate, DateTime endDate, string username)
        {
            var response = jiraHttpRequest.DoJiraRequest<IssuesList>(JiraURIRepository.GET_WORKLOG(startDate, endDate, username), Method.GET);

            List<WorkLog> workLog = new List<WorkLog>();

            foreach (var issue in response.Data.Issues)
            {
                workLog.AddRange(GetIssueWorklog(issue.id));
            }

            workLog.RemoveAll(x => (x.RecordDate < startDate || x.RecordDate > endDate) || x.Author != username);

            List<Task> issuetasks = new List<Task>();
            foreach (var log in workLog)
            {
                issuetasks.Add(Task.Run(() =>
                {
                    var issue = GetIssue(log.IssueId);
                    log.Summary = issue.Summary;
                    log.Key = issue.Key;
                    log.Type = issue.Issuetype;

                    var fields = response.Data.Issues.First(x => x.id == log.IssueId)?.fields;
                    var properties = fields.GetType().GetProperties();
                    log.si3ID = properties.First(p => p.Name == "customfield_10300")?.GetValue(fields)?.ToString().Trim();

                    List<Task> tasks = new List<Task>();

                    if (fields.parent != null && string.IsNullOrEmpty(log.si3ID))
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            var worklogIssueEpica = GetIssue(fields.parent.id);
                            log.si3ID = worklogIssueEpica.si3ID;
                        }));
                    }

                    if (string.IsNullOrEmpty(log.si3ID))
                    {
                        string epicJiraKey = properties.First(p => p.Name == "customfield_10006")?.GetValue(fields)?.ToString().Trim();
                        if (!string.IsNullOrEmpty(epicJiraKey))
                        {
                            tasks.Add(Task.Run(() =>
                            {
                                var worklogIssueEpica = GetIssue(epicJiraKey);
                                log.si3ID = worklogIssueEpica.si3ID;
                            }));
                        }
                    }

                    Task.WhenAll(tasks).Wait();
                }));
            }

            Task.WhenAll(issuetasks).Wait();

            return workLog;
        }

        public List<WorkLog> GetIssueWorklog(string issueKey)
        {
            var response = jiraHttpRequest.DoJiraRequest<JiraWorkLog>(JiraURIRepository.GET_ISSUE_LOG(issueKey), Method.GET);

            return JiraConverter.ConvertWorklog(response.Data);
        }

        public Issue GetIssue(string issueKey)
        {
            var response = jiraHttpRequest.DoJiraRequest<JiraIssue>(JiraURIRepository.GET_ISSUE(issueKey), Method.GET);

            return JiraConverter.ConvertIssue(response.Data);
        }
    }
}
