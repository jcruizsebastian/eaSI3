using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    [Serializable]
    public class Componente
    {
        public List<Modulo> modulos { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}
