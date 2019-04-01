using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<string> Post(Uri uri, Dictionary<string, string> x_www_form_url_encoded = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("cache-control", "no-cache");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("accept-encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "es-ES,es;q=0.5");

            if (x_www_form_url_encoded != null)
            {
                if (x_www_form_url_encoded.ContainsKey("modificaval"))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var value in x_www_form_url_encoded)
                        sb.Append($"{value.Key}={value.Value}&");

                    sb.Remove(sb.Length - 1, 1);

                    request.Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                }
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
