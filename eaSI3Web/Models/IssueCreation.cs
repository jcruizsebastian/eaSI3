using System;

namespace eaSI3Web.Models
{
    public class IssueCreation
    {
        public int IssueCreationId { get; set; }
        public string JiraKey { get; set; }
        public string SI3Key { get; set; }
        public DateTime CreationDate { get; set; }
        public CreationResult CreationResult { get; set; }
        public string CreationResultAddtionalInfo { get; set; }
        public User User { get; set; }
    }
}
