using System.Xml.Serialization;

namespace SI3.Issues
{
    public class SeverityLevel
    {
        [XmlElement("nombre")]
        public string Nombre { get; set; }
        [XmlElement("codigo")]
        public string Codigo { get; set; }
    }
}
