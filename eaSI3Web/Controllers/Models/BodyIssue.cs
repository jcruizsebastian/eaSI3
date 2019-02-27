using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class BodyIssue
    {
        public string JiraKey { get; set; }
        public string Titulo { get; set; }
        public string Prioridad { get; set; }
        public string Tipo { get; set; }
        public string Responsable { get; set; }
        public string Producto { get; set; }
        public string Componente { get; set; }
        public string Modulo { get; set; }
        public string CodUserSi3 { get; set; }
    }
}
