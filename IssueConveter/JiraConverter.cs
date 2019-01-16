using IssueConveter.Model;
using Jira;
using System.Collections.Generic;

namespace IssueConveter
{
    public class JiraConverter
    {
        public static Issue ConvertIssue(JiraIssue data)
        {
            return new Issue() {
                Key = data.key,
                Summary = data.fields.summary
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
