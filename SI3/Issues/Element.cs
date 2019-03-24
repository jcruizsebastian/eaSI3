using System.Xml.Serialization;

namespace SI3.Issues
{
    public class BasicElement : IElement
    {
        [XmlElement("nombre")]
        public string Name { get; set; }
        [XmlElement("codigo")]
        public string Code { get; set; }
    }
}
