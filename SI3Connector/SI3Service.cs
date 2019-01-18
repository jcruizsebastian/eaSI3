using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SI3Connector
{
    public class SI3Service
    {
        private string username { get; set; }
        private string password { get; set; }
        private SI3HttpRequest SI3HttpRequest { get; set; }

        HttpClient client { get; set; }

        public SI3Service(string username, string password)
        {
            this.username = username;
            this.password = password;
            SI3HttpRequest = new SI3HttpRequest();

            Login();
        }

        private string Login()
        {
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Add("user", username);
            x_www_form_url_encoded.Add("pwd", password);
            x_www_form_url_encoded.Add("DSN", "GESOPENFINANCE");

            var request = SI3HttpRequest.Post(new Uri("http://si3.infobolsa.es/si3/asp/identificacion.asp"), x_www_form_url_encoded);
            request.Wait();

            return request.Result;
        }

        public string AddWorklog(string issueid, DateTime date, int time)
        {
            var x_www_form_url_encoded = new Dictionary<string, string>();
            x_www_form_url_encoded.Clear();
            x_www_form_url_encoded.Add("newTimeRecord", time.ToString());
            x_www_form_url_encoded.Add("newDate", $"{date.Day.ToString("D2")}/{date.Month.ToString("D2")}/{date.Year - 2000}");
            x_www_form_url_encoded.Add("timerecordtype", "R");

            var request = SI3HttpRequest.Post(new Uri($"http://si3.infobolsa.es/Si3/its/asp/newTimeRecord.asp?cod={issueid}&type=1"), x_www_form_url_encoded);
            request.Wait();

            return request.Result;
        }
    }
}
