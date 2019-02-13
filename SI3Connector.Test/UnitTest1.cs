using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SI3Connector.Test
{
    [TestClass]
    public class SI3IntegrationTests
    {
        [TestMethod]
        public void AddIssueWork()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            service.AddIssueWork("45817", DateTime.Today, 1);
        }

        [TestMethod]
        public void AddProjectWork()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            Dictionary<DayOfWeek, int> work = new Dictionary<DayOfWeek, int>();
            work.Add(DayOfWeek.Monday, 5);
            work.Add(DayOfWeek.Thursday, 3);

            service.AddProjectWork("H-53", work);
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
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.GetWeekCode(5);
        }

        [TestMethod]
        public void testGetSubproject()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.GetMilestone("O-180,H-10");
        }

        [TestMethod]
        public void testIsIssueOpened()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.IsIssueOpened("44383");
        }

        [TestMethod]
        public void testIsProjectOpened()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.IsProjectOpened("O-180,H-10");
        }

        [TestMethod]
        public void testGetProductos()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.GetProducts();
        }

        [TestMethod]
        public void testGetComponents()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.GetComponents("66");
        }

        [TestMethod]
        public void testGetModules()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.GetModules("449");
        }

        [TestMethod]
        public void testGetUsuarios()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            var a = service.GetUsers();
        }
    }
}
