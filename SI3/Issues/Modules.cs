using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    [XmlRoot("modules")]
    public class Modules
    {
        [XmlElement("module")]
        public List<Module> modules { get; set; }
    }
}
