using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("productos")]
    public class Products : IRepositoryXML<BasicElement>
    {
        [XmlElement("producto")]
        public List<BasicElement> elements { get; set; }
    }
}
