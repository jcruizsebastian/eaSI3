using RestSharp;
using System;
using System.Security.Authentication;

namespace JiraConnector
{
    public class JiraHttpRequest
    {
        private const string jiraURL = "https://jira.openfinance.es/"; //TODO: Esto se debería poder configurar por si finalmente lo usan en infobolsa.

        private string _username { get; set; }
        private string _password { get; set; }

        public JiraHttpRequest(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public IRestResponse<T> DoJiraRequest<T>(string queryStringRequest) where T : new()
        {
            var request = new RestRequest(queryStringRequest, Method.GET);

            request.AddHeader("Authorization", string.Format("Basic {0}", Base64Encode($"{_username}:{_password}")));

            var client = new RestClient(jiraURL);

            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new InvalidCredentialException("Usuario y/o contraseña de JIRA incorrectos.");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException($"Máximo número de intetos de acceso a la API de JIRA excedido. Ingrese nuevamente a través de {jiraURL}.");

            if (response == null || response.Data == null)
                throw new InvalidOperationException("Error with jira API request: " + queryStringRequest);

            return response;
        }


        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
