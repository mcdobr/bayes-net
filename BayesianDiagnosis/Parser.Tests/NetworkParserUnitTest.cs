using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetworkParser;
using BayesianNetwork;
using System.Linq;
using System.Collections.Generic;

namespace BayesianNetwork.Tests
{
    [TestClass]
    public class NetworkParserUnitTest
    {
        static NetworkCreate create = new NetworkCreate();
        static Network mockNetwork = create.startParsing();

        [TestMethod]
        public void TestAskingSimpleQuestion()
        {
            var gripa = mockNetwork.getNodeByName("gripa");
            var q = new Query(gripa, "false", Enumerable.Empty<Fact>());
            Assert.AreEqual(0.9, gripa.ProbabilityDistribution[q], 1e-3);

            var o = new Query(gripa, "true", Enumerable.Empty<Fact>());
            Assert.AreEqual(0.1, gripa.ProbabilityDistribution[o], 1e-3);
        }

        [TestMethod]
        public void TestAskingInferentialQuestion()
        {
            List<Fact> evidence = new List<Fact> { new Fact("anorexie", "true", mockNetwork), new Fact("oboseala", "true", mockNetwork) };

            Node gripa = mockNetwork.Nodes["gripa"];
            Query question = new Query(gripa, "true", evidence);

            Assert.AreEqual(0.39628, mockNetwork.answer(question), 1e-4);
        }
    }
}
