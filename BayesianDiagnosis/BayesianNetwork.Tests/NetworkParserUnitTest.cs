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
            var q = new Query(gripa, "False", Enumerable.Empty<Fact>());
            Assert.AreEqual(0.9, gripa.ProbabilityDistribution[q], 1e-3);

            var o = new Query(gripa, "True", Enumerable.Empty<Fact>());
            Assert.AreEqual(0.1, gripa.ProbabilityDistribution[o], 1e-3);
        }
    }
}
