using System.Collections.Generic;
using System.Xml.Serialization;

namespace SI3.Issues
{
    //[XmlRoot("phases")]
    //public class Phases
    //{
    //    [XmlElement("phase")]
    //    public List<Phase> phase { get; set; }
    //}

    public enum Prioridades
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }
}
