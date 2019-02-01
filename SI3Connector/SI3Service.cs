using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

namespace SI3Connector
{
    public class SI3Service
    {
        private string username { get; set; }
        private string password { get; set; }
        private string usercode { get; set; }
        private SI3HttpRequest SI3HttpRequest { get; set; }

        private HttpClient client { get; set; }

        public SI3Service(string username, string password)
        {
            this.username = username;
            this.password = password;
            SI3HttpRequest = new SI3HttpRequest();

            Login();
        }

        private bool Login()
        {
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Add("user", username);
            x_www_form_url_encoded.Add("pwd", password);
            x_www_form_url_encoded.Add("DSN", "GESOPENFINANCE");

            var request = SI3HttpRequest.Post(new Uri("http://si3.infobolsa.es/si3/asp/identificacion.asp"), x_www_form_url_encoded);
            request.Wait();

            bool accesoCorrecto = false;
            if (!request.Result.Contains("denegado.htm"))
                accesoCorrecto = true;

            usercode = request.Result.Split("code!='")[1].Split("'")[0];

            return accesoCorrecto;
        }

        [XmlRoot("milestones")]
        public class Milestones
        {
            [XmlElement("milestone")]
            public List<Milestone> milestone { get; set; }
        }
        public class Milestone
        {
            [XmlElement("cod")]
            public string Code { get; set; }
            [XmlElement("name")]
            public string Name { get; set; }
            [XmlElement("estado")]
            public string Status { get; set; }
        }

        internal Milestone GetSubproject(string subprojectCode)
        {
            Login();

            var request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/gestion/asp/PlanSistemas.asp"), null);
            request.Wait();

            var doc = new HtmlDocument();
            doc.LoadHtml(request.Result);

            var documentMilestones = doc.DocumentNode.SelectNodes("//*[@id=\"TableExcel\"]/tr/td/table/tr/td/a[@onclick]");
            string documentMilestoneCode = string.Empty;
            for (int i = 4; i < documentMilestones.Count; i = i+2)
            {
                string documentProjectCode = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}][not(@valign)]/td[2]").InnerHtml.Trim();
                if (subprojectCode.Split(",")[0].Replace("-","") == documentProjectCode)
                {
                    documentMilestoneCode = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}]/td/a[@onclick]").OuterHtml.Split("id=\"img")[1].Split("\"></a>")[0].Split("\"")[0];
                    break;
                }
            }

            Login();

            request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/gestion/asp/MilestonesXML.asp?cod={documentMilestoneCode}"), null);
            request.Wait();

            Milestones milestones;

            XmlSerializer serializer = new XmlSerializer(typeof(Milestones));
            using (TextReader reader = new StringReader(request.Result.Replace("\r\n<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?>\r\n", "")))
            {
                milestones = (Milestones)serializer.Deserialize(reader);
            }

            return milestones.milestone.First(x => x.Name.StartsWith(subprojectCode.Split(",")[1].Replace("-", ".")));
        }


        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        internal static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public string AddProjectWork(string issueid, Dictionary<DayOfWeek, int> weekWork)
        {
            Login();
            //var projectCode = GetAllSubprojects().milestone.First(hito => hito.Name.StartsWith(issueid.Replace("-", "."), StringComparison.CurrentCulture)).Code;
            var projectCode = GetSubproject(issueid).Code;
            var weekNumber = GetIso8601WeekOfYear(DateTime.Today);
            var weekCode = GetWeekCode(weekNumber);

            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("modificaval", "YES");
            x_www_form_url_encoded.Add("fweek", string.Empty);

            foreach(var work in weekWork)
            {
                x_www_form_url_encoded.Add($"{projectCode}+++-{weekNumber}-{((int)work.Key)}", work.Value.ToString());
            }

            x_www_form_url_encoded.Add($"COMM{projectCode}+++", string.Empty);

            var request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/treport/asp/saveWReport.asp?cod={weekCode}&stchange=0&initst=1&usercode={usercode}&aa={DateTime.Today.Year}&pn=Resumen"), x_www_form_url_encoded);
            request.Wait();

            return request.Result;
        }

        public string AddIssueWork(string issueid, DateTime date, int time)
        {
            Login();
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("newTimeRecord", time.ToString());
            x_www_form_url_encoded.Add("newDate", $"{date.Day.ToString("D2")}/{date.Month.ToString("D2")}/{date.Year - 2000}");
            x_www_form_url_encoded.Add("timerecordtype", "R");

            var request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/its/asp/newTimeRecord.asp?cod={issueid}&type=1"), x_www_form_url_encoded);
            request.Wait();

            return request.Result;
        }

        internal string GetWeekCode(int weekNumber)
        {
            var request = SI3HttpRequest.Post(new Uri("http://si3.infobolsa.es/Si3/treport/asp/resumen.asp"));
            request.Wait();

            var doc = new HtmlDocument();
            doc.LoadHtml(request.Result);

            var weekCodes = doc.DocumentNode.SelectNodes("/html/body/table/tr/td/a[@onclick]");
            foreach (var week in weekCodes)
            {
                var innerDoc = new HtmlDocument();
                innerDoc.LoadHtml(week.InnerHtml);

                var htmlWeekNumber = innerDoc.DocumentNode.SelectNodes("/b").First().InnerText;
                if (htmlWeekNumber == weekNumber.ToString())
                {
                    if (innerDoc.DocumentNode.InnerHtml.Contains("Submitted"))
                        throw new Exception("Semana ya confirmada.");

                    return week.OuterHtml.Split("openWeek('")[1].Split("'")[0];
                }
            }

            throw new Exception("Semana no dada de alta");
        }
    }
}
