using System.Xml.Serialization;

namespace SI3.Issues
{
    public class UserElement : IElement
    {
        [XmlElement("nombre")]
        public string Name { get; set; }
        [XmlElement("codigo")]
        public string Code { get; set; }
        [XmlElement("codcompose")]
        public string CodCompose { get; set; }
        [XmlElement("codib")]
        public string Codib { get; set; }
    }
}
