using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JiraConnector.Test
{
    [TestClass]
    public class JiraWorkLogServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            JiraWorkLogService jiraWorkLogService = new JiraWorkLogService("jcruiz", "_*_d1d4ct1c95");
            jiraWorkLogService.GetCurrentWeekWorkLog();
        }
    }
}
