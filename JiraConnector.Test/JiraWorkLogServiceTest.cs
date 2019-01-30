using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            //Arrage

            //Act
            var worklog = jiraWorkLogService.GetWorklog(new DateTime(2019, 01, 01), new DateTime(2019, 01, 07), username);

            //Assert

        }
    }
}
