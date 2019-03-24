using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("modules")]
    public class Modules : IRepositoryXML<BasicElement>
    {
        [XmlElement("module")]
        public List<BasicElement> elements { get; set; }
    }
}
