using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eaSI3Web.Controllers.Models
{
    [Serializable]
    public class Producto
    {
        public List<Componente> componentes { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}
