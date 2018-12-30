using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BayesianNetwork;

namespace BayesianNetwork.Tests
{
    [TestClass]
    public class NetworkUnitTest
    {
        static Network mockNetwork;

        [ClassInitialize()]
        public static void SetUp(TestContext context)
        {
            mockNetwork = new Network();

            mockNetwork.addNode("gripa");
            mockNetwork.addNode("abces");
            mockNetwork.addNode("febra");
            mockNetwork.addNode("oboseala");
            mockNetwork.addNode("anorexie");

            // Add edges
            mockNetwork.addLink("gripa", "febra");
            mockNetwork.addLink("abces", "febra");

            mockNetwork.addLink("febra", "oboseala");
            mockNetwork.addLink("febra", "anorexie");

            Query gripaTrueQuery = new Query(mockNetwork.Nodes["gripa"], new Evidence(), "True");
            Query gripaFalseQuery = new Query(mockNetwork.Nodes["gripa"], new Evidence(), "False");

            mockNetwork.Nodes["gripa"].addProbability(gripaTrueQuery, 0.9);
            mockNetwork.Nodes["gripa"].addProbability(gripaFalseQuery, 0.1);
        }

        [TestMethod]
        public void TestAskingQuestions()
        {
            Node gripa = mockNetwork.getNodeByName("gripa");
            Query q = new Query(gripa, new Evidence(), "True");

            Assert.AreEqual(0.9, gripa.ProbabilityDistribution[q], 1e-3);
        }
    }
}
