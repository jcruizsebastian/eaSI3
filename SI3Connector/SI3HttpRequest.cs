using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("SI3Connector.Test")]
namespace SI3Connector
{
    internal class SI3HttpRequest
    {
        private HttpClient httpClient { get; set; }
        private CookieContainer cookieContainer { get; set; }

        public SI3HttpRequest()
        {
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            httpClient = new HttpClient(handler);
        }

        public async Task<string> Post(Uri uri, Dictionary<string,string> x_www_form_url_encoded = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("accept-encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "es-ES,es;q=0.5");

            if(x_www_form_url_encoded != null)
            {
                if(x_www_form_url_encoded.ContainsKey("modificaval"))
                    request.Content = new StringContent("modificaval=YES&fweek=&000990+++-5-1=5&000990+++-5-4=3&COMM000990+++=", Encoding.UTF8, "application/x-www-form-urlencoded");//request.Content = new StringContent("modificaval=YES&fweek=&000990+++-5-1=5&000990+++-5-4=3&COMM000990+++=");
                else
                    request.Content = new FormUrlEncodedContent(x_www_form_url_encoded);
            }
                

            using (var result = await httpClient.SendAsync(request))
            {
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
