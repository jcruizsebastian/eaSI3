using System;

namespace JiraConnector
{
    public class JiraURIRepository
    {
        public static string GET_ISSUE (string issueKey) => $"rest/api/2/issue/{issueKey}";
        public static string GET_ISSUE_LOG (string issueKey) => $"rest/api/2/issue/{issueKey}/worklog";
        public static string GET_WORKLOG(DateTime startDate, DateTime endDate, string username) => $"rest/api/2/search?jql=issueFunction in workLogged(\"by {username} after {startDate.ToString("yyyy/MM/dd")} before {endDate.ToString("yyyy/MM/dd")}\")";
    }
}
