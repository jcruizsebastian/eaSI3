using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("usuarios")]
    public class Usuarios
    {
        [XmlElement("usuario")]
        public List<Usuario> usuario { get; set; }
    }
}
