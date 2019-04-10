using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    public class BodyIssue
    {
        [Required]
        public string JiraKey { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Prioridad { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public string Responsable { get; set; }
        [Required]
        public string Producto { get; set; }
        [Required]
        public string Componente { get; set; }
        public string Modulo { get; set; }
        [Required]
        public string CodUserSi3 { get; set; }
    }
}
