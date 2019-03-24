using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("componentes")]
    public class Components : IRepositoryXML<BasicElement>
    {
        [XmlElement("componente")]
        public List<BasicElement> elements { get; set; }
    }
}
