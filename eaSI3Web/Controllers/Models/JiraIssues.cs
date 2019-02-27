using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class JiraIssues: ICloneable
    {
        public string IssueSI3Code { get; set; }
        public string IssueCode { get; set; }
        public string IssueKey { get; set; }
        public string Titulo { get; set; }
        public double Tiempo { get; set; }

        public object Clone()
        {
            return (JiraIssues)this.MemberwiseClone();
        }
    }
}
