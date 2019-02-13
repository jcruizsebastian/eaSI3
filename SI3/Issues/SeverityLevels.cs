using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    //[XmlRoot("levels")]
    //public class Levels
    //{
    //    [XmlElement("level")]
    //    public List<Level> level { get; set; }
    //}

    public enum SeverityLevels
    {
        Critical = 1,
        Important = 2,
        Minor = 3
    }
}
