using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("componentes")]
    public class Componentes
    {
        [XmlElement("componente")]
        public List<Componente> componente { get; set; }
    }
}
