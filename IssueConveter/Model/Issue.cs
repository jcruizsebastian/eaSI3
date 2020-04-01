using Jira;
using System.Collections.Generic;

namespace IssueConveter.Model
{
    public class Issue
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public string Assignee { get; set; }
        public string Creator { get; set; }
        public int Priority { get; set; }
        public string Issuetype { get; set; }
        public string si3ID { get; set; }
        public string IssueId { get; set; }
        public List<string> Components { get; set; }
        public string Status { get; set; }
        public List<string> FixVersions { get; set; }
    }
}
