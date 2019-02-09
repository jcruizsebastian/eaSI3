using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Projects
{
    [XmlRoot("milestones")]
    public class Milestones
    {
        [XmlElement("milestone")]
        public List<Milestone> milestone { get; set; }
    }
}
