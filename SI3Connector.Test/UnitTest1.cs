using Microsoft.VisualStudio.TestTools.UnitTesting;
using SI3.Issues;
using System;
using System.Collections.Generic;

namespace SI3Connector.Test
{
    [TestClass]
    public class SI3IntegrationTests
    {
        public string _user = "ofjcruiz";
        public string _password = "_*_d1d4ct1c";
        public int _workHours = 40;

        [TestMethod]
        public void AddIssueWork()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            service.AddIssueWork("45817", DateTime.Today, 1);
        }

        [TestMethod]
        public void AddProjectWork()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            Dictionary<DayOfWeek, int> work = new Dictionary<DayOfWeek, int>();
            work.Add(DayOfWeek.Monday, 5);
            work.Add(DayOfWeek.Thursday, 3);

            service.AddProjectWork("O-120,H-53", work);
        }

        [TestMethod]
        public void testrefactor()
        {
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Add("user", "ofjcruiz");
            x_www_form_url_encoded.Add("pwd", "_*_d1d4ct1c");
            x_www_form_url_encoded.Add("DSN", "GESOPENFINANCE");

            SI3HttpRequest request = new SI3HttpRequest();
            request.Post(new Uri("http://si3.infobolsa.es/si3/asp/identificacion.asp"),x_www_form_url_encoded).Wait();

            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("newTimeRecord", "1");
            x_www_form_url_encoded.Add("newDate", $"{DateTime.Today.Day.ToString("D2")}/{DateTime.Today.Month.ToString("D2")}/{DateTime.Today.Year - 2000}");
            x_www_form_url_encoded.Add("timerecordtype", "R");

            request.Post(new Uri("http://si3.infobolsa.es/Si3/its/asp/newTimeRecord.asp?cod=45817&type=1"), x_www_form_url_encoded).Wait();
        }

        [TestMethod]
        public void testGetWeekCode()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.GetWeekCode(5);
        }

        [TestMethod]
        public void testGetSubproject()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.GetMilestone("O-180,H-10");
        }

        [TestMethod]
        public void testIsIssueOpened()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.IsIssueOpened("44383");
        }

        [TestMethod]
        public void testIsProjectOpened()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.IsProjectOpened("O-180,H-10");
        }

        [TestMethod]
        public void testGetProductos()
        {
            SI3Service service = new SI3Service(_user, _password, _workHours, "http://si3.infobolsa.es/");
            var a = service.GetProducts();
        }

        [TestMethod]
        public void testGetComponents()
        {
            SI3Service service = new SI3Service(_user, _password, _workHours, "http://si3.infobolsa.es/");
            var a = service.GetComponents("66");
        }

        [TestMethod]
        public void testGetModules()
        {
            SI3Service service = new SI3Service(_user, _password, _workHours, "http://si3.infobolsa.es/");
            var a = service.GetModules("449");
        }

        [TestMethod]
        public void testGetUsuarios()
        {
            SI3Service service = new SI3Service(_user, _password, _workHours, "http://si3.infobolsa.es/");
            var a = service.GetUsers();
        }

        [TestMethod]
        public void testAvailableHours()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.SpendedHours();
        }

        [TestMethod]
        public void testNewIssue()
        {
            Issue newissue = new Issue()
            {
                title = "Prueba automatica title",//jira key
                cause = "Prueba automatica causa",//titulo de tarea
                level = SeverityLevels.Important, //Cuando la prioridad de jira sea 1 o 2 Minor, 3, Important y 4 y 5 Critical
                phase = Phases.Maintenance, //Cuando el tipo de jira sea Asistencia, preventa entonces Production, cuando sea Desarrollo, Historia, Epica, Pruebas, o Especificacion entonces Development, cuando sea Formaci�n, Gestion, Sistemas entonces user, cuando sea Correcci�n o Bolsa de horas entonces Mantenimiento
                priority = Prioridades.Medium, //Cuando la prioridad de jira sea 1 Low, 2 y 3 Medium, 4 High  y 5 Urgent
                tipo = Tipos.Compatibility, //Cuando sea Manteniemiento => Data_Maintenance, asistencia => Asistencia, Bolsa de hroas => bolsa de horas, Correccion => Defecto, Especificacion => Especificaicon, Formacion => Help_and_Documentation, Gestion => Gestion, Desarollo => Mejora, Preventa =>  Help_and_Documentation, Pruebas => Pruebas , Sistemas => Security, Epica => Mejora
                type = Types.improv, //Cuando el tipo de jira sea Correci�n, enviar error, sino enviar improv
                user = "187",
                product="64",
                component = "591"               
            };

            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.NewIssue(newissue);
        }
        [TestMethod]
        public void GetProjects()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.GetProjects();
        }
        [TestMethod]
        public void GetMilestones()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.GetMilestones();
        }

        [TestMethod]
        public void GetJustAddeProjectWorkTest()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c", 40, "http://si3.infobolsa.es/");
            var a = service.GetAlreadyTimeRecorded("47983-10");
        }

        [TestMethod]
        public void SubmitTest()
        {
            SI3Service service = new SI3Service(_user, _password, _workHours, "http://si3.infobolsa.es/");
            service.Submit();
        }
    }
    
}
