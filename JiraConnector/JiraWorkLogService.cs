using IssueConveter;
using IssueConveter.Model;
using Jira;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JiraConnector
{
    public class JiraWorkLogService
    {
        private JiraHttpRequest jiraHttpRequest { get; set; }

        public JiraWorkLogService(string username, string password)
        {
            jiraHttpRequest = new JiraHttpRequest(username, password);

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

            foreach (var log in workLog)
            {
                var issue = GetIssue(log.IssueId);
                log.Summary = issue.Summary;
                log.Key = issue.Key;

                log.si3ID = response.Data.Issues.First(x => x.id == log.IssueId)?.fields?.customfield_10300?.ToString().Trim();
                if (string.IsNullOrEmpty(log.si3ID))
                {
                    string epicJiraKey = response.Data.Issues.First(y => y.id == log.IssueId)?.fields?.customfield_10006?.ToString().Trim();
                    if (!string.IsNullOrEmpty(epicJiraKey))
                    {
                        var worklogIssueEpica = GetIssue(epicJiraKey);
                        log.si3ID = worklogIssueEpica.si3ID;
                    }
                }
            }

            //workLog.RemoveAll(x => string.IsNullOrEmpty(x.si3ID));
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
