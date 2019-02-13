using System;
using System.Collections.Generic;
using System.Text;

namespace SI3.Issues
{
    public class NewIssue
    {
        public string product { get; set; }
        public string component { get; set; }
        public string title { get; set; }
        public string cause { get; set; }
        public string dversionXX = "00";
        public string dversionYY = "00";
        public string dversion = "00";
        public string module = "0";
        public string countrycode = "0";
        public string os = "0";

        //Este va en texto plano
        public enum Types
        {
            improv, //enhancement
            error //defect
        }

        public Types type { get; set; }
        public Tipos tipo { get; set; }
        public Phases phase { get; set; }
        public SeverityLevels level { get; set; }
        public Prioridades priority { get; set; }
    }
}
