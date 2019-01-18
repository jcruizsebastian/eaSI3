using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SI3Connector.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SI3Service service = new SI3Service("ofjcruiz", "_*_d1d4ct1c");
            service.Login().Wait();
            service.AddWorklog("45817", DateTime.Today, 1).Wait();
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
    }
}
