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
using eaSI3Web.Controllers.Models;
using Project = SI3Connector.Model.Project;
using Microsoft.Extensions.Options;
using eaSI3Web.Configs;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;

namespace eaSI3Web.Controllers
{
    [Route("api/[controller]")]
    public class SI3Controller : Controller
    {
        private readonly StatisticsContext _context;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IOptions<Data> data;
        public SI3Controller(StatisticsContext context, IOptions<Data> data)
        {
            this.data = data;
            _context = context;
        }
        public static List<Models.Producto> products { get; set; }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<List<Models.Producto>> Products()
        {
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));

            products = new List<Models.Producto>();
            var productos = new Dictionary<String,String>();
            try
            {
                SI3Service SI3Service = new SI3Service("ofjcruiz", "_*_d1d4ct1c",data.Value.Week_Hours,data.Value.Si3_Host_URL);
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

                    if (product.Key.StartsWith("F"))
                    {
                        products.Add(new Models.Producto()
                        {
                            name = "Formación e investigación",
                            code = product.Value,
                            componentes = components
                        });
                    }
                    else if (product.Key.StartsWith("Coord"))
                    {
                        products.Add(new Models.Producto()
                        {
                            name = "Coordinación y gestión",
                            code = product.Value,
                            componentes = components
                        });
                    }
                    else {
                        products.Add(new Models.Producto()
                        {
                            name = product.Key, code = product.Value, componentes = components
                        });
                    }
                }
            } catch (InvalidCredentialException e) {
                logger.Error("Username: " + user.SI3UserName + " ,Error: " + e.Message);
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
        private T DeSerializeObject<T>(string fileName)
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
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<List<Models.User>> Users()
        {
            List<Models.User> users = new List<Models.User>();
            try
            {
                SI3Service SI3Service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", data.Value.Week_Hours, data.Value.Si3_Host_URL);
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
                logger.Error("Error: " + e.Message);
                return StatusCode(401, e.Message);
            }

            return users.OrderBy(x => x.nombre).ToList();
        }
        [HttpGet("[action]")]
        public ActionResult<int> AvailableHours(string selectedWeek) {

            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));
            try
            {
                SI3Service Si3Service = new SI3Service(user.SI3UserName, bdStatistics.DecryptSi3Password(user.SI3UserName), data.Value.Week_Hours, data.Value.Si3_Host_URL);
                var a = Si3Service.SpendedHours(Convert.ToInt32(selectedWeek)).Sum(x => x.Value);
                return a;
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (InvalidCredentialException e)
            {
                logger.Error("Username: " + user.SI3UserName + " ,Error: " + e.Message);
                return StatusCode(401, e.Message);
            }
        }
        [HttpPost("[action]")]
        public ActionResult ValidateLogin()
        {
            var idUser = "";

            if (Request.Cookies.ContainsKey("userId"))
            {
                var cookie = Request.Cookies.First(x => x.Key == "userId");
                idUser = cookie.Value;
            }
            else
            {
                return StatusCode(400, "Por favor, inicia sesión de nuevo");
            }

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));
            try
            {
                SI3Service sI3Service = new SI3Service(user.SI3UserName, bdStatistics.DecryptSi3Password(user.SI3UserName), data.Value.Week_Hours, data.Value.Si3_Host_URL);
            }
            catch (InvalidCredentialException e)
            {
                logger.Error("Username: " + user.JiraUserName + " ,Error: " + e.Message);
                return StatusCode(401, e.Message);
            }

            return Ok();
        }
        [HttpPost("[action]")]
        public ActionResult<int> Login([FromBody] BodyData bodyData)
        {
            int id=int.MinValue;
            try
            {
                SI3Service si3Service = new SI3Service(bodyData.usernameSi3, bodyData.passwordSi3, data.Value.Week_Hours, data.Value.Si3_Host_URL);
                if (!string.IsNullOrEmpty(bodyData.usernameSi3))
                {
                    BdStatistics bdStatistics = new BdStatistics(_context);
                    bdStatistics.AddUser(bodyData.usernameJira,bodyData.passwordJira,bodyData.usernameSi3,bodyData.passwordSi3);
                    bdStatistics.AddLogin(bodyData.usernameSi3);
                    id = bdStatistics.GetUserId(bodyData.usernameSi3);
                }
            }
            catch (InvalidCredentialException e)
            {
                logger.Error("Username: " +bodyData.usernameJira + " ,Error: " + e.Message);
                return StatusCode(401, e.Message);
            }

            return id;
        }

        [HttpPost("[action]")]
        public ActionResult<string> Linkissue([FromBody]BodyIssue data)
        {
            string NewIssue = "";
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));
            try
            {
                SI3.Issues.Issue issue = new SI3.Issues.Issue();
                SI3Service SI3Service = new SI3Service(user.SI3UserName, bdStatistics.DecryptSi3Password(user.SI3UserName), this.data.Value.Week_Hours, this.data.Value.Si3_Host_URL);
                issue.user = data.CodUserSi3;
                issue.product = data.Producto;
                issue.component = data.Componente;
                if (data.Modulo != "default") { issue.module = data.Modulo; }
                issue.title = data.JiraKey.ToUpper() + " - " + RemoveDiacritics(data.Titulo);
                issue.cause = RemoveDiacritics(data.Titulo);

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
                    case "Data_Maintenance":
                        issue.tipo = Tipos.Data_Maintenance;
                        break;
                    case "Asistencia":
                        issue.tipo = Tipos.Asistencia;
                        break;
                    case "Bolsa_de_horas":
                        issue.tipo = Tipos.Bolsa_de_horas;
                        break;
                    case "Defecto":
                        issue.tipo = Tipos.Defecto;
                        break;
                    case "Especificacion":
                        issue.tipo = Tipos.Especificacion;
                        break;
                    case "Help_and_Documentation":
                        issue.tipo = Tipos.Help_and_Documentation;
                        break;
                    case "Gestion":
                        issue.tipo = Tipos.Gestion;
                        break;
                    case "Mejora":
                        issue.tipo = Tipos.Mejora;
                        break;
                    case "Pdte_asignacion_proyecto":
                        issue.tipo = Tipos.Pdte_asignacion_proyecto;
                        break;
                    case "Pruebas":
                        issue.tipo = Tipos.Pruebas;
                        break;
                    case "Security":
                        issue.tipo = Tipos.Security;
                        break;
                    case "Performance":
                        issue.tipo = Tipos.Performance;
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
                logger.Error("Username: " + user.SI3UserName + " ,Error: " + e.Message);
                return StatusCode(401, e.Message);
            }
            return NewIssue;
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<List<Milestones>> Milestones()
        {
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));
            List<Milestones> milestones = new List<Milestones>();
            SI3Service Si3Service = new SI3Service(user.SI3UserName, bdStatistics.DecryptSi3Password(user.SI3UserName), data.Value.Week_Hours, data.Value.Si3_Host_URL);
            Dictionary<string, List<SI3.Projects.Milestone>> milestonesService = Si3Service.GetMilestones();
            foreach (var m in milestonesService)
            {
                Milestones milestone = new Milestones
                {
                    ProjectCode = m.Key,
                    Milestone = m.Value
                };
                milestones.Add(milestone);
            }
            return milestones;
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<List<Models.Project>> Projects()
        {
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));
            List<Models.Project> projects = new List<Models.Project>();
            try
            {
                SI3Service Si3Service = new SI3Service(user.SI3UserName, bdStatistics.DecryptSi3Password(user.SI3UserName), data.Value.Week_Hours, data.Value.Si3_Host_URL);
                List<Project> projectsService = Si3Service.GetProjects();
                foreach (var proj in projectsService)
                {
                    Models.Project project = new Models.Project
                    {
                        code = proj.Code,
                        title = proj.Title
                    };

                    projects.Add(project);
                }
            }
            catch (InvalidCredentialException e)
            {
                logger.Error("Username: " + user.SI3UserName + " ,Error: " + e.Message);
                return StatusCode(401,e.Message);
            }

            return projects;
        }
        [HttpGet("[action]")]
        public ActionResult editIssue(string id, int tipo, int user)
        {
            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User usuario = bdStatistics.GetUser(user);

            try
            {
                SI3Service service = new SI3Service(usuario.SI3UserName, bdStatistics.DecryptSi3Password(usuario.SI3UserName),0,data.Value.Si3_Host_URL);
                service.EditIssue(id,tipo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }
        [HttpGet("[action]")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<List<Tipo>> getTypes()
        {
            List<Tipo> tipos = new List<Tipo>();

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User usuario = bdStatistics.GetUser(1);

            foreach (int i in Enum.GetValues(typeof(Tipos)))
            {
                var name = Enum.GetName(typeof(Tipos), i);
                tipos.Add(new Tipo { name = name, cod = i});
            }
            return tipos;
        }
        [HttpPost("[action]")]
        public ActionResult Submit([FromBody] BodyData body)
        {
            try
            {
                SI3Service si3Service = new SI3Service(body.usernameSi3, body.usernameSi3, data.Value.Week_Hours, data.Value.Si3_Host_URL);
                //si3Service.Submit();
            }
            catch (InvalidCredentialException e)
            {
                return StatusCode(401, e.Message);
            }

            return Ok();
        }
        [HttpPost("[action]")]
        public ActionResult Register([FromQuery]string selectedWeek, [FromQuery]int totalHours, [FromBody]IEnumerable<WeekJiraIssues> model, bool submit)
        {
            var cookie = Request.Cookies.First(x => x.Key == "userId");
            var idUser = cookie.Value;
            int weekNumber = Convert.ToInt32(selectedWeek);

            BdStatistics bdStatistics = new BdStatistics(_context);
            eaSI3Web.Models.User user = bdStatistics.GetUser(int.Parse(idUser));

            try
            {
                SI3Service SI3Service = new SI3Service(user.SI3UserName, bdStatistics.DecryptSi3Password(user.SI3UserName), data.Value.Week_Hours, data.Value.Si3_Host_URL);

                foreach (var week in model.ToList())
                {
                    week.Issues.RemoveAll(x => x.Tiempo == 0 || string.IsNullOrEmpty(x.IssueSI3Code));
                    week.Issues.ForEach(x => x.IssueSI3Code = x.IssueSI3Code.Trim().Split(';')[x.IssueSI3Code.Trim().Split(';').Length -1].Trim());
                }

                if (submit && !model.SelectMany(x => x.Issues).Any())
                {
                    SI3Service.Submit(weekNumber);
                    return Ok();
                }

                var a = ValidarImputación(SI3Service, model);
                a.Wait();
                
                var validacion = a.Result;
                                 
                if (validacion.Count() != 0)
                    throw new SI3Exception(validacion);

                model = NormalizarHoras(model, SI3Service, weekNumber);

                Dictionary<string, Dictionary<DayOfWeek, int>> weekWork = new Dictionary<string, Dictionary<DayOfWeek, int>>();

                List<Task> tasks = new List<Task>();
                foreach (var dateIssue in model)
                {
                    foreach (var issue in dateIssue.Issues)
                    {
                        int timeToInt = (int)issue.Tiempo; //SI3 no permite horas parciales, solo horas enteras.
                        int idNumber;

                        if (Int32.TryParse(issue.IssueSI3Code, out idNumber))
                        {

                            tasks.Add(Task.Run(() =>SI3Service.AddIssueWork(issue.IssueSI3Code, dateIssue.Fecha, timeToInt)));
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

                Task.WhenAll(tasks).Wait();

                foreach (var week in weekWork)
                {
                    SI3Service.AddProjectWork(week.Key, week.Value, weekNumber);
                }

                if (submit && a.IsCompletedSuccessfully)
                    SI3Service.Submit(weekNumber);
            }
            catch (SI3Exception e)
            {
                logger.Error("Username: " + user.SI3UserName + " ,Error: " + e.Message);
                bdStatistics.AddWorkTracking(user.SI3UserName, weekNumber, totalHours, 1, e.Message);
                return StatusCode(400, e.errors);
            }

            bdStatistics.AddWorkTracking(user.SI3UserName, weekNumber, totalHours, 0, "Horas imputadas en Si3 correctamente");
            return Ok();
        }

        private async Task<IEnumerable<string>> ValidarImputación(SI3Service sI3Service, IEnumerable<WeekJiraIssues> model)
        {
            //StringBuilder sb = new StringBuilder();
            List<Task> tasks = new List<Task>();

            IList<string> errors = new List<string>();

            var issuesIds = model.SelectMany(x => x.Issues).Where(z => z.Tiempo > 0).Select(y => y.IssueSI3Code);

            foreach (var issueid in issuesIds)
            {                
                if (double.TryParse(issueid, out var a))
                {
                    tasks.Add(Task.Run(() =>
                    {
                        if (!sI3Service.IsIssueOpened(issueid))
                            errors.Add($"La issue con id {issueid} no está abierta, revise Si3.");
                            //sb.AppendLine($"La issue con id {issueid} no existe o está cerrada.");
                    }));
                }
                else
                {
                    //var userProjects = sI3Service.GetProjectsUser();

                    tasks.Add(Task.Run(() =>
                    {

                        if (!sI3Service.existsProject(issueid) || !sI3Service.IsProjectOpened(issueid))
                           errors.Add($"El proyecto con id {issueid} no está abierto o no tienes permisos para imputar en él, revise Si3.");
                        //else
                        //    if (!userProjects.Select(x => x.Code).Contains(issueid))
                        //        errors.Add($"El proyecto con id {issueid} no está asociado a su usuario.");
                    }));
                }
            }

            await Task.WhenAll(tasks);
            return errors.Distinct();
        }

        private IEnumerable<WeekJiraIssues> NormalizarHoras(IEnumerable<WeekJiraIssues> tareasSinNormalizar, SI3Service SI3Service, int weekNumber)
        {
            var issues = tareasSinNormalizar.SelectMany(x => x.Issues).ToList();
            var issuesQueue = new LinkedList<JiraIssues>(issues);

            var availableHours = SI3Service.SpendedHours(weekNumber);

            IList<WeekJiraIssues> normalized = new List<WeekJiraIssues>();

            foreach (var hours in availableHours) 
            {
                var availableHoursInDay = 8 - hours.Value;
                while(availableHoursInDay != 0)
                {
                    if (!issuesQueue.Any())
                        break;

                    var dequedIssue = issuesQueue.First();
                    issuesQueue.RemoveFirst();

                    if (dequedIssue.Tiempo > availableHoursInDay)
                    {
                        var splitedIssue = (JiraIssues)dequedIssue.Clone();
                        splitedIssue.Tiempo = Math.Abs(dequedIssue.Tiempo - availableHoursInDay);
                        dequedIssue.Tiempo = availableHoursInDay;
                        issuesQueue.AddFirst(splitedIssue);
                    }

                    var currentDay = StartOfWeek(tareasSinNormalizar.First().Fecha, DayOfWeek.Monday);
                    currentDay = currentDay.AddDays((int)hours.Key-1);
                    if (!normalized.Any(x => x.Fecha == currentDay))
                        normalized.Add(new WeekJiraIssues() { Fecha = currentDay, Issues = new List<JiraIssues>() });

                    normalized.Where(x => x.Fecha == currentDay).First().Issues.Add(dequedIssue);
                    availableHoursInDay = availableHoursInDay - (int)dequedIssue.Tiempo;
                }
            }

            return normalized;
        }

        private DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        private static string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
