using eaSI3Web.Models;
using System;
using System.Linq;

namespace eaSI3Web.Controllers.UsageStatistics
{
    public class BdStatistics
    {
        private readonly StatisticsContext _context;

        public BdStatistics(StatisticsContext context)
        {
            _context = context;
        }

        public void AddUser(string username)
        {
            var users = from u in _context.Users where u.SI3UserName.Equals(username) select u;

            if (!users.Any())
            {
                _context.Add(new User() { JiraUserName = username.Substring(2,username.Length - 2) , SI3UserName = username });
                _context.SaveChanges();
            }
        }

        public void AddLogin(string username)
        {
            var users = from u in _context.Users where u.SI3UserName.CompareTo(username) == 0 select u;

            _context.Add(new Login() { User = (User)users.First(), ConnectionDate = DateTime.Now });
            _context.SaveChanges();
        }

        public void AddIssueCreation(string username, string jiraKey, string si3Key, int error, string message)
        {
            var users = from u in _context.Users where u.JiraUserName.CompareTo(username) == 0 select u;
            IssueCreation issueCreation = new IssueCreation()
            {
                CreationDate = DateTime.Now,
                JiraKey = jiraKey,
                SI3Key = si3Key,
                CreationResultAddtionalInfo = message,
                User = users.First(),
                CreationResult = (CreationResult)error
            };

            _context.Add(issueCreation);
            _context.SaveChanges();
        }

        public void AddWorkTracking(string username, int week, int totalHours, int error, string message)
        {
            var users = from u in _context.Users where u.SI3UserName.CompareTo(username) == 0 select u;
            WorkTracking workTracking = new WorkTracking()
            {
                TotalHours = totalHours,
                Week = week,
                User = users.First(),
                Year = DateTime.Now.Year,
                TrackingDate = DateTime.Now,
                TrackResultAddtionalInfo = message,
                TrackResult = (TrackResult)error
            };

            _context.Add(workTracking);
            _context.SaveChanges();
        }
    }
}
