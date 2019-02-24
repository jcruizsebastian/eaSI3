using System;

namespace eaSI3Web.Models
{
    public class Login
    {
        public int LoginId { get; set; }
        public User User { get; set; }
        public DateTime ConnectionDate { get; set; }
    }
}
