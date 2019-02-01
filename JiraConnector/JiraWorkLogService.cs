﻿using IssueConveter;
using IssueConveter.Model;
using Jira;
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
        }

        public List<WorkLog> GetWorklog(DateTime startDate, DateTime endDate, string username)
        {
            var response = jiraHttpRequest.DoJiraRequest<ListIssues>(JiraURIRepository.GET_WORKLOG(startDate, endDate, username));

            List<WorkLog> workLog = new List<WorkLog>();

            foreach (var issue in response.Data.Issues)
            {
                workLog.AddRange(GetIssueWorklog(issue.id));
            }

            workLog.RemoveAll(x => (x.RecordDate < startDate || x.RecordDate > endDate) || x.Author != username);

            foreach (var log in workLog)
            {
                log.si3ID = response.Data.Issues.First(x => x.id == log.IssueId)?.fields?.customfield_10300?.ToString().Trim();
                var issue = GetIssue(log.IssueId);
                log.Summary = issue.Summary;
                log.Key = issue.Key;
            }

            workLog.RemoveAll(x => string.IsNullOrEmpty(x.si3ID));

            return workLog;
        }

        public List<WorkLog> GetIssueWorklog(string issueKey)
        {
            var response = jiraHttpRequest.DoJiraRequest<JiraWorkLog>(JiraURIRepository.GET_ISSUE_LOG(issueKey));

            return JiraConverter.ConvertWorklog(response.Data);
        }

        public Issue GetIssue(string issueKey)
        {
            var response = jiraHttpRequest.DoJiraRequest<JiraIssue>(JiraURIRepository.GET_ISSUE(issueKey));

            return JiraConverter.ConvertIssue(response.Data);
        }
    }
}
