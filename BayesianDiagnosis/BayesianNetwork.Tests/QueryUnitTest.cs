using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BayesianNetwork.Tests
{
    /// <summary>
    /// Summary description for QueryUnitTest
    /// </summary>
    [TestClass]
    public class QueryUnitTest
    {
        [TestMethod]
        public void TestQueriesAreEqual()
        {
            Node mockNode = new Node("mock");
            Query q = new Query(Enumerable.Empty<EvidenceItem>(), "True");
            Query o = new Query(Enumerable.Empty<EvidenceItem>(), "True");

            Console.WriteLine(q.GetHashCode());
            Console.WriteLine(o.GetHashCode());

            Assert.AreEqual(q, o);
        }
    }
}
