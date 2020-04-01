using IssueConveter.Model;
using Jira;
using System;
using System.Linq;
using System.Collections.Generic;

namespace IssueConveter
{
    public class JiraConverter
    {
        public static Issue ConvertIssue(JiraIssue data)
        {
            List<string> fixVersions = new List<string>();
            if (data.fields.fixVersions != null && data.fields.fixVersions.Count > 0)
            {
                fixVersions = data.fields.fixVersions.Cast<Dictionary<string, object>>().Select(x => (string)x["name"]).ToList();
            }

            List<string> components = new List<string>();
            if (data.fields.components != null && data.fields.components.Count > 0)
            {
                fixVersions = data.fields.components.Select(x => x.name).ToList();
            }

            return new Issue() {
                Key = data.key,
                Summary = data.fields.summary,
                Assignee = data.fields.assignee?.name,
                Creator = data.fields.creator.name,
                Priority = Int32.Parse(data.fields.priority.id),
                Issuetype = data.fields.issuetype.name,
                IssueId = data.id,
                si3ID = data.fields.customfield_10300?.ToString(),
                Components = components,
                Status = data.fields.status.name,
                FixVersions = fixVersions
            };
        }

        public static List<WorkLog> ConvertWorklog(JiraWorkLog data)
        {
            var worklogs = new List<WorkLog>();

            foreach (var log in data.worklogs)
            {
                var worklog = new WorkLog() {
                    IssueId = log.issueId,
                    Author = log.author.key,
                    TimeSpentSeconds = log.timeSpentSeconds,
                    TimeSpent = log.timeSpent,
                    Comment = log.comment,
                    RecordDate = log.started
                };

                worklogs.Add(worklog);
            }

            return worklogs;
        }
    }
}
