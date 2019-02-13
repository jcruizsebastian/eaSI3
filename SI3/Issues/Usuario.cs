using System.Xml.Serialization;

namespace SI3.Issues
{
    public class Usuario
    {
        [XmlElement("nombre")]
        public string Nombre { get; set; }
        [XmlElement("codigo")]
        public string Codigo { get; set; }
        [XmlElement("codcompose")]
        public string Codcompose { get; set; }
        [XmlElement("codib")]
        public string Codib { get; set; }
    }
}
