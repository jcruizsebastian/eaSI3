using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("usuarios")]
    public class Users : IRepositoryXML<BasicElement>
    {
        [XmlElement("usuario")]
        public List<BasicElement> elements { get; set; }
    }
}
