using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class BodyData
    {
        [Required]
        public string usernameJira { get; set; }
        [Required]
        public string passwordJira { get; set; }
        [Required]
        public string usernameSi3 { get; set; }
        [Required]
        public string passwordSi3 { get; set; }
    }
}
