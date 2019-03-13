namespace eaSI3Web.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string JiraUserName { get; set; }
        public string SI3UserName { get; set; }
        public string JiraPassword { get; set; }
        public string SI3Password { get; set; }
    }
}
