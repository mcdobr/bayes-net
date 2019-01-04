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
        public void TestQueriesShouldBeEqual()
        {
            Network mockNetwork = new Network();
            Node mockNode = new Node("mock", mockNetwork);

            Query one = new Query(mockNode, "True", Enumerable.Empty<Fact>());
            Query two = new Query(mockNode, "True", Enumerable.Empty<Fact>());
            Assert.AreEqual(one, two);
        }

        [TestMethod]
        public void TestQueriesShouldNotBeEqual()
        {
            Network mockNetwork = new Network();
            Node mockNode = new Node("mock", mockNetwork);

            Query one = new Query(mockNode, "True", Enumerable.Empty<Fact>());
            Query two = new Query(mockNode, "False", Enumerable.Empty<Fact>());
            Assert.AreNotEqual(one, two);
        }

        [TestMethod]
        public void TestQueriesShouldHaveSameHashCode()
        {
            Network mockNetwork = new Network();
            Node mockNode = new Node("mock", mockNetwork);

            Query one = new Query(mockNode, "True", Enumerable.Empty<Fact>());
            Query two = new Query(mockNode, "True", Enumerable.Empty<Fact>());

            Assert.AreEqual(one.GetHashCode(), two.GetHashCode());
        }
    }
}
