using Jira;

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
    }
}
