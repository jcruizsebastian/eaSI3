﻿using HtmlAgilityPack;
using SI3.Issues;
using SI3.Projects;
using SI3Connector.Exceptions;
using SI3Connector.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SI3Connector
{
    public class SI3Service
    {
        private string usercode { get; set; }
        private SI3HttpRequest SI3HttpRequest { get; set; }
        private int Work_Hours { get; set; }
        private string si3Url { get; set; }
        public SI3Service(string username, string password, int Work_Hours, string si3Url)
        {
            SI3HttpRequest = new SI3HttpRequest();
            this.Work_Hours = Work_Hours;
            this.si3Url = si3Url;
            Login(username, password);
        }

        private static Dictionary<string, string> products { get; set; }
        public Dictionary<string, string> GetProducts()
        {
            if (products != null)
                return products;

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/ProductosActivosXML.asp"), null);
            request.Wait();

            var productos = GetGeneric<Productos>(request.Result);

            products = productos.producto.ToDictionary(x => x.Nombre, y => y.Codigo);

            return products;
        }

        public void Submit()
        {
            int WORK_HOURS = Work_Hours;

            var spendedHours = SpendedHours().Sum(x => x.Value);
            if (spendedHours != WORK_HOURS)
                throw new SI3Exception($"No se pueden consignar menos de {WORK_HOURS} horas.");

            var request = SI3HttpRequest.Post(new Uri($""), null);
            request.Wait();
        }

        private static Dictionary<string, Dictionary<string, string>> components { get; set; }
        public Dictionary<string, string> GetComponents(string producto)
        {
            if (components != null && components.ContainsKey(producto))
                return components[producto];

            if(components == null)
                components = new Dictionary<string, Dictionary<string, string>>();

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/ComponentesXML.asp?cod={producto}"), null);
            request.Wait();

            var componentes = GetGeneric<Componentes>(request.Result);

            components[producto] = componentes.componente.ToDictionary(x => x.Nombre, y => y.Codigo);

            return components[producto];
        }

        private static Dictionary<string, Dictionary<string, string>> modules { get; set; }
        public Dictionary<string, string> GetModules(string component)
        {
            if (modules != null && modules.ContainsKey(component))
                return modules[component];

            if (modules == null)
                modules = new Dictionary<string, Dictionary<string, string>>();

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/ModulesXML.asp?cod={component}"), null);
            request.Wait();

            var modulos = GetGeneric<Modules>(request.Result);

            modules[component] = modulos.modules.ToDictionary(x => x.Nombre, y => y.Codigo);

            return modules[component];
        }

        private static Dictionary<string, string> users { get; set; }
        public Dictionary<string, string> GetUsers()
        {
            if (users != null)
                return users;

            //Login();

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/usuariosFiltradosXML.asp"), null);
            request.Wait();

            var usuarios = GetGeneric<Usuarios>(request.Result);

            users = usuarios.usuario.ToDictionary(x => x.Nombre, y => y.Codigo);

            return users;
        }

        public void Login(string username, string password)
        {
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Add("user", username);
            x_www_form_url_encoded.Add("pwd", password);
            x_www_form_url_encoded.Add("DSN", "GESOPENFINANCE");

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}si3/asp/identificacion.asp"), x_www_form_url_encoded);
            request.Wait();

            if (request.Result.Contains("denegado.htm"))
                throw new InvalidCredentialException("Usuario y/o contraseña de SI3 incorrectos.");

            usercode = request.Result.Split("code!='")[1].Split("'")[0];
        }

        public string NewIssue(Issue newIssueData)
        {
            //Login();
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("product", newIssueData.product);
            x_www_form_url_encoded.Add("component", newIssueData.component);
            x_www_form_url_encoded.Add("dversionXX", newIssueData.dversionXX);
            x_www_form_url_encoded.Add("dversionYY", newIssueData.dversionYY);
            x_www_form_url_encoded.Add("dversion", newIssueData.dversion);
            x_www_form_url_encoded.Add("module", newIssueData.module);
            x_www_form_url_encoded.Add("countrycode", newIssueData.countrycode);
            x_www_form_url_encoded.Add("os", newIssueData.os);
            x_www_form_url_encoded.Add("type", ((int)newIssueData.type).ToString());
            x_www_form_url_encoded.Add("types", ((int)newIssueData.tipo).ToString());
            x_www_form_url_encoded.Add("phase", ((int)newIssueData.phase).ToString());
            x_www_form_url_encoded.Add("prior", ((int)newIssueData.priority).ToString());
            x_www_form_url_encoded.Add("level", ((int)newIssueData.level).ToString());
            x_www_form_url_encoded.Add("title", newIssueData.title);
            x_www_form_url_encoded.Add("cause", newIssueData.cause);

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/CreateIssue.asp"), x_www_form_url_encoded);
            request.Wait();

            var idSi3 = request.Result.Split("viewToEdit('")[1].Split("'")[0];

            //Login();

            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("isid", idSi3);
            x_www_form_url_encoded.Add("asign", newIssueData.user);

            var request2 = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/saveIssue.asp"), x_www_form_url_encoded);
            request2.Wait();

            //Login();

            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("isid", idSi3);
            x_www_form_url_encoded.Add("actions", newIssueData.actions);

            var request3 = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/saveIssue.asp"), x_www_form_url_encoded);
            request3.Wait();

            return idSi3;
        }

        public string AddIssueWork(string issueid, DateTime date, int time)
        {
            //Login();
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("newTimeRecord", time.ToString());
            x_www_form_url_encoded.Add("newDate", $"{date.Day.ToString("D2")}/{date.Month.ToString("D2")}/{date.Year - 2000}");
            x_www_form_url_encoded.Add("timerecordtype", "R");

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/newTimeRecord.asp?cod={issueid}&type=1"), x_www_form_url_encoded);
            request.Wait();

            return request.Result;
        }

        public bool IsIssueOpened(string issueid)
        {
            //Login();
            Regex regex = new Regex(@"^[0-9]+$");
            Match match = regex.Match(issueid);

            if (!match.Success)
                throw new SI3Exception("El ID SI3 de la tarea: " + issueid + " es incorrecto!");

            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Clear();

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/its/asp/viewIssue.asp?cod={issueid}"), x_www_form_url_encoded);
            request.Wait();

            return request.Result.Contains("Open&nbsp;&nbsp");
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

            var justAddedWork = GetJustAddeProjectWork(weekCode);
            foreach (var addedWork in justAddedWork)
            {
                foreach(var work in addedWork.Value)
                {
                    var key = $"{addedWork.Key}+++-{weekNumber}-{((int)work.Key)}";
                    if (x_www_form_url_encoded.ContainsKey(key))
                        x_www_form_url_encoded[key] = (Convert.ToInt32(x_www_form_url_encoded[key]) + Convert.ToInt32(work.Value)).ToString();
                    else
                        x_www_form_url_encoded.Add(key, work.Value.ToString());
                }
            }

            x_www_form_url_encoded.Add($"COMM{projectCode}+++", string.Empty);

            //Login();

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/treport/asp/saveWReport.asp?cod={weekCode}&stchange=0&initst=1&usercode={usercode}&aa={DateTime.Today.Year}&pn=Resumen"), x_www_form_url_encoded);
            request.Wait();

            return request.Result;
        }

        public Dictionary<string, Dictionary<DayOfWeek, int>> GetJustAddeProjectWork(string weekCode)
        {
            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/treport/asp/weeklyreport.asp?cod={weekCode}&aa={DateTime.Today.Year}&pn=Resumen"));
            request.Wait();

            var doc = new HtmlDocument();
            doc.LoadHtml(request.Result);

            var documentMilestones = doc.DocumentNode.SelectNodes("//*/tr[contains(@name,\"rowproyecto\")]/td/input[contains(@onclick,\"selectObj(this)\")]");
            return documentMilestones
                .Where(x => x.Attributes["value"].Value != "0")
                .GroupBy(y => y.Attributes["id"].Value.Split("-")[0])
                .ToDictionary(  z => z.Key.Trim(), 
                                s => s.ToDictionary(p => (DayOfWeek)Int32.Parse(p.Attributes["id"].Value.Split("-")[2]), 
                                                    h => Int32.Parse(h.Attributes["value"].Value)));
        }

        public bool IsProjectOpened(string issueid)
        {
            try
            {
                Regex regex = new Regex(@"^O\-[0-9]+\,H\-[0-9]+$");
                Match match = regex.Match(issueid);
                if (!match.Success)
                    throw new SI3Exception("El ID SI3 del proyecto: " + issueid + " es incorrecto!");

                GetMilestone(issueid);

            }catch(SI3Exception si3Exception)
            {
                return false;
            }

            return true;
        }

        internal Milestone GetMilestone(string milestoneProjectCode)
        {
            var projectCode = milestoneProjectCode.Split(",")[0].Replace("-", "");
            var milestoneCode = milestoneProjectCode.Split(",")[1].Replace("-", ".");

            //Login();

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/gestion/asp/PlanSistemas.asp"), null);
            request.Wait();

            string projectID = GetProjectId(projectCode, request.Result);

            //Login();

            request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/gestion/asp/MilestonesXML.asp?cod={projectID}"), null);
            request.Wait();
            var result = request.Result;

            return GetMilestone(milestoneProjectCode, milestoneCode, result);
        }


        private static T GetGeneric<T>(string result)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(result.Replace("\r\n<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?>\r\n", "").Replace("<?xml-stylesheet type=\"text/xsl\" href=\"usuarios.xsl\"?>", "").Replace("<?xml version='1.0'?>", "")))
            {
                return (T)serializer.Deserialize(reader);
            }
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
                    throw new SI3Exception($"Proyect {milestoneProjectCode} no encontrado.");
                }

                if (milestone.Status != "I")
                {
                    throw new SI3Exception($"Es imposible imputar en el proyecto {milestoneProjectCode} pues no está abierto.");
                }

                return milestone;
            }
        }
        //Método para sacar todos los hitos
        public Dictionary<string,List<Milestone>> GetMilestones()
        {
            Dictionary<string,List<Milestone>> milestones = new Dictionary<string, List<Milestone>>();
            List<Project> projects = GetProjects();
            foreach (var project in projects)
            {
                var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/gestion/asp/MilestonesXML.asp?cod={project.Id}"), null);
                request.Wait();

                XmlSerializer serializer = new XmlSerializer(typeof(Milestones));
                using (TextReader reader = new StringReader(request.Result.Replace("\r\n<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?>\r\n", "")))
                {
                    Milestones milestonesSerializer = (Milestones)serializer.Deserialize(reader);
                    List<Milestone> milestone = milestonesSerializer.milestone;
                    milestone.RemoveAll(x => x.Status != "I");
                    if (milestone.Count > 0)
                    {
                        milestones.Add(project.Code, milestone);
                    }
                }                
            }
            return milestones;
        }
        //Método para sacar todos los proyectos
        public List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/gestion/asp/PlanSistemas.asp"), null);
            request.Wait();

            var doc = new HtmlDocument();
            doc.LoadHtml(request.Result);

            var documentMilestones = doc.DocumentNode.SelectNodes("//*[@id=\"TableExcel\"]/tr/td/table/tr/td/a[@onclick]");
            for (int i = 4; i < documentMilestones.Count; i = i+2)
            {
                var milestone = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}][not(@valign)]/td[2]");
                if (milestone != null)
                {
                    string projectCode = milestone.InnerHtml.Trim();
                    string titleProject = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}]/td[3]/a/u").InnerHtml.Trim();
                    var projectID = GetProjectId(projectCode, request.Result);

                    Project project = new Project
                    {
                        Code = projectCode,
                        Title = titleProject,
                        Id = projectID
                    };

                    projects.Add(project);
                }
            }
            return projects;
        }
        private static string GetProjectId(string project, string result)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            var documentMilestones = doc.DocumentNode.SelectNodes("//*[@id=\"TableExcel\"]/tr/td/table/tr/td/a[@onclick]");
            string documentMilestoneCode = string.Empty;
            for (int i = 4; i < documentMilestones.Count; i = i + 2)
            {
                var milestone = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}][not(@valign)]/td[2]");

                string projectCode = milestone.InnerHtml.Trim();
                string titleProject = milestone.Attributes["title"].Value.Trim();

                if (project == projectCode)
                {
                    documentMilestoneCode = doc.DocumentNode.SelectSingleNode($"//*[@id=\"TableExcel\"]/tr/td/table/tr[{i}]/td/a[@onclick]").OuterHtml.Split("id=\"img")[1].Split("\"></a>")[0].Split("\"")[0];
                    break;
                }
            }

            if (string.IsNullOrEmpty(documentMilestoneCode))
                throw new SI3Exception($"No se encontró el projecto con código {project}");

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

        public Dictionary<DayOfWeek, int> SpendedHours()
        {
            Dictionary<DayOfWeek, int> availableHours = new Dictionary<DayOfWeek, int>();

            var weekNumber = GetIso8601WeekOfYear(DateTime.Today);
            var weekCode = GetWeekCode(weekNumber);
            //var weekCode = "47806-9";

            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/treport/asp/weeklyreport.asp?cod={weekCode}&aa={DateTime.Today.Year}&pn=Resumen"));
            request.Wait();

            var doc = new HtmlDocument();
            doc.LoadHtml(request.Result);

            var weekHours = doc.DocumentNode.SelectNodes("//td[@colspan=2][contains(text(),'Totals')]/parent::*/td[not(@colspan)]/input");

            for(int contador = 0; contador < 5; contador++)
            {
                availableHours.Add((DayOfWeek)contador + 1, Convert.ToInt32(weekHours[contador].Attributes["value"].Value));
            }

            return availableHours;
        }

        internal string GetWeekCode(int weekNumber)
        {
            var request = SI3HttpRequest.Post(new Uri($"{si3Url}Si3/treport/asp/resumen.asp"));
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
