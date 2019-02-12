using HtmlAgilityPack;
using SI3.Projects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Xml.Serialization;

namespace SI3Connector
{
    public class SI3Service
    {
        private string username { get; set; }
        private string password { get; set; }
        private string usercode { get; set; }
        private SI3HttpRequest SI3HttpRequest { get; set; }

        public SI3Service(string username, string password)
        {
            this.username = username;
            this.password = password;
            SI3HttpRequest = new SI3HttpRequest();
        }

        //TODO: Sacar método para comprobar que existen la issues y los proyectos y que están abiertos. 

        private void Login()
        {
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Add("user", username);
            x_www_form_url_encoded.Add("pwd", password);
            x_www_form_url_encoded.Add("DSN", "GESOPENFINANCE");

            var request = SI3HttpRequest.Post(new Uri("http://si3.infobolsa.es/si3/asp/identificacion.asp"), x_www_form_url_encoded);
            request.Wait();

            if (request.Result.Contains("denegado.htm"))
                throw new InvalidCredentialException("Usuario y/o contraseña de SI3 incorrectos.");

            usercode = request.Result.Split("code!='")[1].Split("'")[0];
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

        public string AddProjectWork(string issueid, Dictionary<DayOfWeek, int> weekWork)
        {
            var projectCode = GetMilestone(issueid).Code;
            var weekNumber = GetIso8601WeekOfYear(DateTime.Today);
            var weekCode = GetWeekCode(weekNumber);

            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("modificaval", "YES");
            x_www_form_url_encoded.Add("fweek", string.Empty);

            foreach (var work in weekWork)
            {
                x_www_form_url_encoded.Add($"{projectCode}+++-{weekNumber}-{((int)work.Key)}", work.Value.ToString());
            }

            x_www_form_url_encoded.Add($"COMM{projectCode}+++", string.Empty);

            Login();

            var request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/treport/asp/saveWReport.asp?cod={weekCode}&stchange=0&initst=1&usercode={usercode}&aa={DateTime.Today.Year}&pn=Resumen"), x_www_form_url_encoded);
            request.Wait();

            return request.Result;
        }

        internal Milestone GetMilestone(string milestoneProjectCode)
        {
            var projectCode = milestoneProjectCode.Split(",")[0].Replace("-", "");
            var milestoneCode = milestoneProjectCode.Split(",")[1].Replace("-", ".");

            Login();

            var request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/gestion/asp/PlanSistemas.asp"), null);
            request.Wait();

            string projectID = GetProjectId(projectCode, request.Result);

            Login();

            request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/gestion/asp/MilestonesXML.asp?cod={projectID}"), null);
            request.Wait();
            var result = request.Result;

            return GetMilestone(milestoneProjectCode, projectCode, result);
        }

        private static Milestone GetMilestone(string milestoneProjectCode, string projectCode, string result)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Milestones));
            using (TextReader reader = new StringReader(result.Replace("\r\n<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?>\r\n", "")))
            {
                Milestones milestones = (Milestones)serializer.Deserialize(reader);
                Milestone milestone = milestones.milestone.FirstOrDefault(x => x.Name.StartsWith(projectCode));

                if (milestone == null)
                {
                    throw new Exception($"Proyect {milestoneProjectCode} no encontrado.");
                }

                if (milestone.Status != "A") //TODO: Comprobar con que letra encaja
                {
                    throw new Exception($"Es imposible imputar en el proyecto {milestoneProjectCode} pues no está abierto.");
                }

                return milestone;
            }
        }

        private static string GetProjectId(string project, string result)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            var documentMilestones = doc.DocumentNode.SelectNodes("//*[@id=\"TableExcel\"]/tr/td/table/tr/td/a[@onclick]");
            string documentMilestoneCode = string.Empty;
            for (int i = 4; i < documentMilestones.Count; i = i + 2)
            {
                string documentProjectCode = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}][not(@valign)]/td[2]").InnerHtml.Trim();
                if (project == documentProjectCode)
                {
                    documentMilestoneCode = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}]/td/a[@onclick]").OuterHtml.Split("id=\"img")[1].Split("\"></a>")[0].Split("\"")[0];
                    break;
                }
            }

            if (string.IsNullOrEmpty(documentMilestoneCode))
                throw new Exception($"No se encontró el projecto con código {project}");

            return documentMilestoneCode;
        }

        internal static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
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
