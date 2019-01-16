using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
    }
}
