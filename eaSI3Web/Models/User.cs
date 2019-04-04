namespace eaSI3Web.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string JiraUserName { get; set; }
        public string SI3UserName { get; set; }
        public byte[] Password_Encrypted { get; set; }
        public byte[] PasswordSi3_Encrypted { get; set; }
    }
}
