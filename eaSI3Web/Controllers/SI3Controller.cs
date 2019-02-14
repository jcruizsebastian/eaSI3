﻿using System;
using System.Collections.Generic;
using System.Linq;
using SI3Connector;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static eaSI3Web.Controllers.JiraController;
using Microsoft.Extensions.Logging;
using System.Text;
using SI3Connector.Exceptions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class SI3Controller : Controller
    {
        private readonly ILogger<SI3Controller> _logger;
        public SI3Controller(ILogger<SI3Controller> logger) {
            _logger = logger;
        }

        public class Issue
        {
            public string product { get; set; }
            public string component { get; set; }
            public string module = "0";
            public string title { get; set; }
            public string cause { get; set; }
            public string user { get; set; }
            public string type { get; set; }
            public string tipo { get; set; }
            public string phase { get; set; }
            public string level { get; set; }
            public string priority { get; set; }
        }

        [Serializable]
        public class Producto
        {
            [Serializable]
            public class Componente
            {
                [Serializable]
                public class Modulo
                {
                    public string code { get; set; }
                    public string name { get; set; }
                }

                public List<Modulo> modulos { get; set; }
                public string code { get; set; }
                public string name { get; set; }
            }

            public List<Componente> componentes { get; set; }
            public string code { get; set; }
            public string name { get; set; }
        }

        public static List<Producto> products { get; set; }

        [HttpGet("[action]")]
        public List<Producto> Products([FromQuery]string username, [FromQuery]string password)
        {
            if (products != null)
                return products;

            try
            {
                products = DeSerializeObject<List<Producto>>("productosSerializados.xml");
                return products;
            }
            catch (Exception e)
            {
                //esto de que esté vacío es temporal
            }

            products = new List<Producto>();

            SI3Service SI3Service = new SI3Service(username, password);
            var productos = SI3Service.GetProducts();
            foreach(var product in productos)
            {                
                List<Producto.Componente> components = new List<Producto.Componente>();

                var componentes = SI3Service.GetComponents(product.Value);
                foreach(var componente in componentes)
                {
                    List<Producto.Componente.Modulo> modules = new List<Producto.Componente.Modulo>();

                    var modulos = SI3Service.GetModules(componente.Value);

                    foreach (var modulo in modulos)
                    {
                        modules.Add(new Producto.Componente.Modulo() { name = modulo.Key, code = modulo.Value});
                    }

                    components.Add(new Producto.Componente() { name = componente.Key, code = componente.Value, modulos = modules });
                }

                products.Add(new Producto() { name = product.Key, code = product.Value, componentes = components });
            }


            return products;
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }


        [HttpPost("[action]")]
        public string LinkIssue([FromQuery]string username, [FromQuery]string password, [FromBody]Issue issue)
        {



            return string.Empty;
        }

        [HttpPost("[action]")]
        public string Register([FromQuery]string username, [FromQuery]string password, [FromQuery]string selectedWeek,[FromQuery]int totalHours,[FromBody]IEnumerable<WeekJiraIssues> model)
        {
            int error = 0;

            try
            {
                _logger.LogInformation("Usuario " + username + " hizo clic en el botón de Enviar Si3 ");
                SI3Service SI3Service = new SI3Service(username, password);

                var validacion = ValidarImputación(SI3Service, model);
                if (!string.IsNullOrEmpty(validacion))
                    throw new SI3Exception(validacion);

                NormalizarHoras(model);                

                Dictionary<string, Dictionary<DayOfWeek, int>> weekWork = new Dictionary<string, Dictionary<DayOfWeek, int>>();

                foreach (var dateIssue in model)
                {

                    foreach (var issue in dateIssue.Issues)
                    {
                        int timeToInt = (int)issue.Tiempo; //SI3 no permite horas parciales, solo horas enteras.
                        int idNumber;

                        if (Int32.TryParse(issue.IssueSI3Code, out idNumber))
                        {
                            SI3Service.AddIssueWork(issue.IssueSI3Code, dateIssue.Fecha, timeToInt);
                        }
                        else
                        {
                            if (!weekWork.ContainsKey(issue.IssueSI3Code))
                            {
                                weekWork.Add(issue.IssueSI3Code, new Dictionary<DayOfWeek, int>());
                            }

                            Dictionary<DayOfWeek, int> dayWork = weekWork[issue.IssueSI3Code];

                            if (!dayWork.ContainsKey(dateIssue.Fecha.DayOfWeek))
                            {
                                dayWork.Add(dateIssue.Fecha.DayOfWeek, timeToInt);
                            }
                            else
                            {
                                dayWork[dateIssue.Fecha.DayOfWeek] += timeToInt;
                            }
                        }
                    }
                }

                foreach (var week in weekWork)
                {
                    SI3Service.AddProjectWork(week.Key, week.Value);
                }
            }
            catch (SI3Exception e)
            {
                _logger.LogError("Usuario : " + username + " Error : " + e.Message);
                error = 1;
                return e.Message;

            }
            catch (Exception e)
            {
                _logger.LogError("Usuario : " + username + " Error : " + e.Message);
                error = 1;
                return e.Message;

            }
            finally {
                new BDPrueba(username,selectedWeek,DateTime.Now.ToShortTimeString(),totalHours,error).Conexion();
            }

            _logger.LogInformation("Usuario : " + username + ", horas imputadas en Si3 correctamente");
            return string.Empty;
        }

        private string ValidarImputación(SI3Service sI3Service, IEnumerable<WeekJiraIssues> model)
        {
            StringBuilder sb = new StringBuilder();

            var issuesIds = model.SelectMany(x => x.Issues).Where(z => z.Tiempo > 0).Select(y => y.IssueSI3Code);

            foreach (var issueid in issuesIds)
            {
                if (double.TryParse(issueid, out var a))
                {
                    if (!sI3Service.IsIssueOpened(issueid))
                        sb.AppendLine($"La issue con id {issueid} no existe o está cerrada.");
                }
                else
                {
                    if (!sI3Service.IsProjectOpened(issueid))
                        sb.AppendLine($"El proyecto con id {issueid} no existe o está cerrado.");
                }
            }

            return sb.ToString();
        }

        public void NormalizarHoras(IEnumerable<WeekJiraIssues> tareasSinNormalizar)
        {
            var diasConHorasDeMas = tareasSinNormalizar.Where(x => x.Issues.Sum(y => (y.Tiempo)) > 8);
            var diasConHorasDeMenos = tareasSinNormalizar.Where(x => x.Issues.Sum(y => (y.Tiempo)) < 8);
            Queue<WeekJiraIssues.JiraIssues> sobrante = new Queue<WeekJiraIssues.JiraIssues>();

            foreach (var diaConSobrante in diasConHorasDeMas)
            {
                double horas_sobrantes = diaConSobrante.Issues.Sum(x => x.Tiempo) - 8;

                var issuesConHorasSobrantes = diaConSobrante.Issues.Where(x => x.Tiempo >= (horas_sobrantes));

                if (issuesConHorasSobrantes != null && issuesConHorasSobrantes.Any())
                {
                    var issue = issuesConHorasSobrantes.First();

                    for (int i = 0; i < horas_sobrantes; i++)
                    {
                        var clonedIssue = (WeekJiraIssues.JiraIssues)issue.Clone();
                        clonedIssue.Tiempo = 1;
                        sobrante.Enqueue(clonedIssue);
                    }

                    issue.Tiempo -= horas_sobrantes;
                }
                else //Solo tiene issues de 1 hora
                {
                    var issues = diaConSobrante.Issues.Where(x => x.Tiempo == 1).ToList().Take((int)horas_sobrantes);
                    foreach (var issue in issues)
                    {
                        sobrante.Enqueue(issue);
                        diaConSobrante.Issues.Remove(issue);
                    }
                }
            }

            if (!sobrante.Any())
                return;

            foreach (var diaConFaltante in diasConHorasDeMenos)
            {
                double horas_faltantes = Math.Abs(diaConFaltante.Issues.Sum(x => x.Tiempo) - 8);

                for (int i = 0; i < horas_faltantes; i++)
                {
                    var issue = sobrante.Dequeue();
                    diaConFaltante.Issues.Add(issue);
                }
            }
        }
    }
}
