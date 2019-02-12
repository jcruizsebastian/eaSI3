using System.Xml.Serialization;

namespace SI3.Projects
{
    public class Milestone
    {
        [XmlElement("cod")]
        public string Code { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("estado")]
        public string Status { get; set; }
    }
}
