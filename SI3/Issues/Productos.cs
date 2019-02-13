using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("productos")]
    public class Productos
    {
        [XmlElement("producto")]
        public List<Producto> producto { get; set; }
    }
}
