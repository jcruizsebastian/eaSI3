﻿using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Security.Authentication;

namespace JiraConnector
{
    public class JiraHttpRequest
    {
        private string jiraURL { get; set; } 
        private string _userName { get; set; }
        private string _password { get; set; }

        public JiraHttpRequest(string userName, string password, string _jiraURL)
        {
            jiraURL = _jiraURL;
            _userName = userName;
            _password = password;
        }

        public IRestResponse<T> DoJiraRequest<T>(string queryStringRequest, Method method, string bodyJson = null) where T : new()
        {
            var request = new RestRequest(queryStringRequest, method);

            if (!string.IsNullOrEmpty(bodyJson))
            {
                request.AddParameter("application/json", bodyJson, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
            }

            var client = new RestClient(jiraURL)
            {
                Authenticator = new HttpBasicAuthenticator(_userName, _password)
            };

            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new InvalidCredentialException("Usuario y/o contraseña de JIRA incorrectos.");

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new UnauthorizedAccessException($"Máximo número de intentos de acceso a la API de JIRA excedido. Ingrese nuevamente a través de {jiraURL}.");

            if (response == null || !(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent))
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
