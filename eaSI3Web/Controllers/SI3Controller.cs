using eaSI3Web.Controllers.UsageStatistics;
using eaSI3Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SI3.Issues;
using SI3Connector;
using SI3Connector.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using static eaSI3Web.Controllers.JiraController;
using eaSI3Web.Controllers.Models;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class SI3Controller : Controller
    {
        private readonly ILogger<SI3Controller> _logger;
        private readonly StatisticsContext _context;

        public SI3Controller(ILogger<SI3Controller> logger, StatisticsContext context)
        {
            _logger = logger;
            _context = context;
        }
        public static List<Models.Producto> products { get; set; }

        [HttpGet("[action]")]
        public ActionResult<List<Models.Producto>> Products([FromQuery]string username, [FromQuery]string password)
        {

/*#if DEBUG
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
                //return StatusCode(500,e.Message);
            }
#endif*/
            products = new List<Models.Producto>();
            var productos = new Dictionary<String,String>();
            try
            {
                SI3Service SI3Service = new SI3Service("jcruiz", "_*_d1d4ct1c");
                productos = SI3Service.GetProducts();
                foreach (var product in productos)
                {
                    List<Models.Componente> components = new List<Models.Componente>();

                    var componentes = SI3Service.GetComponents(product.Value);
                    foreach (var componente in componentes)
                    {
                        List<Models.Modulo> modules = new List<Models.Modulo>();

                        var modulos = SI3Service.GetModules(componente.Value);

                        foreach (var modulo in modulos)
                        {
                            modules.Add(new Models.Modulo() { name = modulo.Key, code = modulo.Value });
                        }

                        components.Add(new Models.Componente() { name = componente.Key, code = componente.Value, modulos = modules });
                    }

                    products.Add(new Models.Producto() { name = product.Key, code = product.Value, componentes = components });
                }
            } catch (InvalidCredentialException e) {
                return StatusCode(401, e.Message);
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

        [HttpGet("[action]")]
        public ActionResult<List<Models.User>> Users()
        {
            List<Models.User> users = new List<Models.User>();
            try
            {
                SI3Service SI3Service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
                foreach (var userDic in SI3Service.GetUsers())
                {
                    Models.User user = new Models.User
                    {
                        nombre = userDic.Key,
                        codigo = userDic.Value

                    };
                    users.Add(user);
                }
            }
            catch (InvalidCredentialException e)
            {
                return StatusCode(401, e.Message);
            }

            return users.OrderBy(x => x.nombre).ToList();
        }

        [HttpGet("[action]")]
        public ActionResult ValidateLogin(string username, string password)
        {

            try
            {
                SI3Service si3Service = new SI3Service(username, password);
                si3Service.Login();
                if (!string.IsNullOrEmpty(username))
                {
                    BdStatistics bdStatistics = new BdStatistics(_context);
                    bdStatistics.AddUser(username);
                    bdStatistics.AddLogin(username);
                }
            }
            catch (InvalidCredentialException e)
            {
                return StatusCode(401, e.Message);
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public ActionResult<string> Linkissue([FromQuery]string username, [FromQuery]string password, [FromBody]BodyIssue data)
        {
            string NewIssue = "";

            try
            {
                SI3.Issues.Issue issue = new SI3.Issues.Issue();
                SI3Service SI3Service = new SI3Service(username, password);
                issue.user = data.CodUserSi3;
                issue.product = data.Producto;
                issue.component = data.Componente;
                if (data.Modulo != "default") { issue.module = data.Modulo; }
                issue.title = data.JiraKey.ToUpper();
                issue.cause = data.Titulo;

                switch (data.Prioridad)
                {
                    case "Trivial":
                    case "Menor":
                        issue.level = SeverityLevels.Minor;
                        break;
                    case "Mayor":
                        issue.level = SeverityLevels.Important;
                        break;
                    case "Crítica":
                    case "Bloqueadora":
                        issue.level = SeverityLevels.Critical;
                        break;
                    default:
                        issue.level = SeverityLevels.Minor;
                        break;
                }

                switch (data.Prioridad)
                {
                    case "Trivial":
                        issue.priority = Prioridades.Low;
                        break;
                    case "Menor":
                        issue.priority = Prioridades.Medium;
                        break;
                    case "Mayor":
                    case "Crítica":
                        issue.priority = Prioridades.High;
                        break;
                    case "Bloqueadora":
                        issue.priority = Prioridades.Urgent;
                        break;
                    default:
                        issue.priority = Prioridades.Medium;
                        break;
                }

                switch (data.Tipo)
                {
                    case "Asistencia":
                    case "Preventa":
                        issue.phase = Phases.Production;
                        break;
                    case "Desarrollo":
                    case "Tarea":
                    case "Historia":
                    case "Épica":
                    case "Pruebas":
                    case "Especificación":
                    case "Análisis":
                    case "Workpack":
                    case "Calidad":
                    case "Change Request":
                        issue.phase = Phases.Development;
                        break;
                    case "Formación":
                    case "Riesgo":
                    case "Vacaciones":
                    case "Interno":
                    case "Gestión":
                    case "Sistemas":
                    case "Permisos":
                        issue.phase = Phases.User;
                        break;
                    case "Corrección":
                    case "Bolsa de horas":
                        issue.phase = Phases.Maintenance;
                        break;
                    default:
                        issue.phase = Phases.Maintenance;
                        break;
                }

                switch (data.Tipo)
                {
                    case "Mantenimiento":
                        issue.tipo = Tipos.Data_Maintenance;
                        break;
                    case "Asistencia":
                        issue.tipo = Tipos.Asistencia;
                        break;
                    case "Bolsa de Horas":
                        issue.tipo = Tipos.Bolsa_de_horas;
                        break;
                    case "Corrección":
                        issue.tipo = Tipos.Defecto;
                        break;
                    case "Especificación":
                    case "Análisis":
                        issue.tipo = Tipos.Especificacion;
                        break;
                    case "Formación":
                        issue.tipo = Tipos.Help_and_Documentation;
                        break;
                    case "Gestión":
                    case "Vacaciones":
                        issue.tipo = Tipos.Gestion;
                        break;
                    case "Desarrollo":
                    case "Tarea":
                    case "Workpack":
                    case "Calidad":
                    case "Change Request":
                        issue.tipo = Tipos.Mejora;
                        break;
                    case "Preventa":
                        issue.tipo = Tipos.Help_and_Documentation;
                        break;
                    case "Pruebas":
                        issue.tipo = Tipos.Pruebas;
                        break;
                    case "Sistemas":
                    case "Interno":
                        issue.tipo = Tipos.Security;
                        break;
                    case "Épica":
                    case "Permisos":
                        issue.tipo = Tipos.Mejora;
                        break;
                    default:
                        issue.tipo = Tipos.Mejora;
                        break;
                }

                if (data.Tipo == "Corrección") { issue.type = Types.error; } else { issue.type = Types.improv; }
                NewIssue = SI3Service.NewIssue(issue);
            }
            catch (InvalidCredentialException e)
            {
                return StatusCode(401, e.Message);
            }
            return NewIssue;
        }
        [HttpPost("[action]")]
        public ActionResult Register([FromQuery]string username, [FromQuery]string password, [FromQuery]string selectedWeek, [FromQuery]int totalHours, [FromBody]IEnumerable<WeekJiraIssues> model)
        {
            BdStatistics bdStatistics = new BdStatistics(_context);
            try
            {
                _logger.LogInformation("Usuario " + username + " hizo clic en el botón de Enviar Si3 ");
                SI3Service SI3Service = new SI3Service(username, password);

                foreach (var week in model.ToList())
                {
                    week.Issues.RemoveAll(x => x.Tiempo == 0 || string.IsNullOrEmpty(x.IssueSI3Code));
                }

                if (!model.SelectMany(x => x.Issues).Any())
                    throw new Exception("No existen tareas con id de SI a imputar.");

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
                bdStatistics.AddWorkTracking(username, int.Parse(selectedWeek), totalHours, 1, e.Message);
                return StatusCode(400, "Error :" + e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Usuario : " + username + " Error : " + e.Message);
                bdStatistics.AddWorkTracking(username, int.Parse(selectedWeek), totalHours, 1, e.Message);
                if (e is InvalidCredentialException) { return StatusCode(401, e.Message); }
                return StatusCode(400, "Error :" + e.Message);
            }


            bdStatistics.AddWorkTracking(username, int.Parse(selectedWeek), totalHours, 0, "Horas imputadas en Si3 correctamente");
            _logger.LogInformation("Usuario : " + username + ", horas imputadas en Si3 correctamente");
            return Ok();
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
