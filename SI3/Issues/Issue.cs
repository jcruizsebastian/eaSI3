namespace SI3.Issues
{
    public class Issue
    {
        public string product { get; set; }
        public string component { get; set; }
        public string module = "0";
        public string title { get; set; }
        public string cause { get; set; }
        public string user { get; set; }
        public readonly string dversionXX = "00";
        public readonly string dversionYY = "00";
        public readonly string dversion = "00";
        public readonly string countrycode = "0";
        public readonly string os = "0";
        public readonly string actions = "OP";

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
