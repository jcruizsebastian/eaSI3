using System;

namespace Jira
{
    public class WorkLog
    {
        public JiraWorkLog.Author Author { get; set; }
        public string TimeSpent { get; set; }
        public int TimeSpentSeconds { get; set; }
        public string Comment { get; set; }
        public DateTime RecordDate { get; set; }
        public string IssueId { get; set; }
        public string Key { get; set; }
        public string Summary { get; set; }
        public string si3ID { get; set; }
    }
}
