using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Security.Authentication;

namespace JiraConnector.Test
{
    [TestClass]
    public class JiraWorkLogServiceTest
    {
        private string username { get; set; }
        private string password { get; set; }
        private JiraWorkLogService jiraWorkLogService { get; set; }

        [TestInitialize]
        public void InitializeTests()
        {
            username = "jcruiz";
            password = "_*_d1d4ct1c97";

            jiraWorkLogService = new JiraWorkLogService(username, password);
        }

        [TestMethod]
        public void TestGetCurrentWorklog()
        {
            //Act
            var worklog = jiraWorkLogService.GetWorklog(new DateTime(2019, 01, 01), new DateTime(2019, 01, 07), username);

            //Assert
            Assert.IsNotNull(worklog, "No se recibió issue alguna");
        }

        [TestMethod]
        public void TestGetIssue()
        {
            //Arrage
            string jiraIssueKey = "CORPORT-401";

            //Act
            var issue = jiraWorkLogService.GetIssue(jiraIssueKey);

            //Assert
            Assert.IsNotNull(issue, $"No se recibió información de la issue {jiraIssueKey}");
        }

        [TestMethod]
        public void TestLogin()
        {
            //Assert
            jiraWorkLogService = new JiraWorkLogService(username, password);
        }

        [TestMethod]
        public void TestBadLogin()
        {
            //Arrange
            string badusername = "anyuser";
            string badpassword = "anypass";

            //Assert
            Assert.ThrowsException<InvalidCredentialException>(() => new JiraWorkLogService(badusername, badpassword));
        }

        [TestMethod]
        public void TestUpdateIssue()
        {
            //Arrage
            string idSI3 = "1111";
            string jiraKey = "CORPORT-401";
            string body = JsonConvert.SerializeObject(new { fields = new { customfield_10300 = idSI3 } });

            //Act
            jiraWorkLogService.UpdateIssue(jiraKey, body);
        }
    }
}
