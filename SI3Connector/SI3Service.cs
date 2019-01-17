using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SI3Connector
{
    public class SI3Service
    {
        public string username { get; set; }
        public string password { get; set; }

        HttpClient client { get; set; }

        public SI3Service(string username, string password)
        {
            this.username = username;
            this.password = password;

            
        }

        public async Task<string> Login()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("user", username);
            dict.Add("pwd", password);
            dict.Add("DSN", "GESOPENFINANCE");

            var request = new HttpRequestMessage(HttpMethod.Post, "http://si3.infobolsa.es/si3/asp/identificacion.asp");
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("accept-encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "es-ES,es;q=0.5");
            request.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            
            request.Content = new FormUrlEncodedContent(dict);

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                HttpClient client = new HttpClient(handler);

                using (var result = await client.SendAsync(request))
                {
                    string content = await result.Content.ReadAsStringAsync();

                    IEnumerable<Cookie> responseCookies = cookieContainer.GetCookies(new Uri("http://si3.infobolsa.es/si3/asp/identificacion.asp")).Cast<Cookie>();
                    foreach (Cookie cookie in responseCookies)
                    {
                        Debug.WriteLine(cookie.Name + ": " + cookie.Value);

                        cookiea = cookie;
                    }
                        


                    return content;
                }
            }

            //var response = await client.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{
            //}
            //else
            //{

            //}
        }

        public Cookie cookiea { get; set; }

        public async Task<string> AddWorklog(string issueid, DateTime date, int time)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("newTimeRecord", time.ToString());
            dict.Add("newDate", $"{date.Day.ToString("D2")}/{date.Month.ToString("D2")}/{date.Year - 2000}");
            dict.Add("timerecordtype", "R");

            var request = new HttpRequestMessage(HttpMethod.Post, $"http://si3.infobolsa.es/Si3/its/asp/newTimeRecord.asp?cod={issueid}&type=1");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Accept-Language", "es-ES,es;q=0.5");
            request.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

            request.Content = new FormUrlEncodedContent(dict);

            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                cookieContainer.Add(cookiea);

                HttpClient client = new HttpClient(handler);

                using (var result = await client.SendAsync(request))
                {
                    string content = await result.Content.ReadAsStringAsync();
                    return content;
                }
            }
        }
    }
}
