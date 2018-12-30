using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BayesianNetwork;
using System.Linq;
using System.Collections.Generic;

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

            mockNetwork.addNode("gripa")
                .addNode("abces")
                .addNode("febra")
                .addNode("oboseala")
                .addNode("anorexie");
                
            mockNetwork.addLink("gripa", "febra")
                .addLink("abces", "febra")
                .addLink("febra", "oboseala")
                .addLink("febra", "anorexie");


            var gripa = mockNetwork.Nodes["gripa"];
            gripa.addProbability(new Query(Enumerable.Empty<EvidenceItem>(), "False"), 0.9);
            gripa.addProbability(new Query(Enumerable.Empty<EvidenceItem>(), "True"), 0.1);


            var abces = mockNetwork.Nodes["abces"];
            abces.addProbability(new Query(Enumerable.Empty<EvidenceItem>(), "False"), 0.95);
            abces.addProbability(new Query(Enumerable.Empty<EvidenceItem>(), "True"), 0.05);

            var febra = mockNetwork.Nodes["febra"];
            var gripaFalse = new EvidenceItem(gripa, "False", mockNetwork);
            var gripaTrue = new EvidenceItem(gripa, "True", mockNetwork);
            var abcesFalse = new EvidenceItem(abces, "False", mockNetwork);
            var abcesTrue = new EvidenceItem(abces, "True", mockNetwork);

            
            febra.addProbability(new Query(new List<EvidenceItem> { gripaFalse, abcesFalse }, "False"), 0.95);
            febra.addProbability(new Query(new List<EvidenceItem> { gripaFalse, abcesFalse }, "True"), 0.05);

            febra.addProbability(new Query(new List<EvidenceItem> { gripaFalse, abcesTrue }, "False"), 0.75);
            febra.addProbability(new Query(new List<EvidenceItem> { gripaFalse, abcesTrue }, "True"), 0.25);

            febra.addProbability(new Query(new List<EvidenceItem> { gripaTrue, abcesFalse }, "False"), 0.3);
            febra.addProbability(new Query(new List<EvidenceItem> { gripaTrue, abcesFalse }, "True"), 0.7);

            febra.addProbability(new Query(new List<EvidenceItem> { gripaTrue, abcesTrue }, "False"), 0.2);
            febra.addProbability(new Query(new List<EvidenceItem> { gripaTrue, abcesTrue }, "True"), 0.8);


            var oboseala = mockNetwork.Nodes["oboseala"];
            var febraTrue = new EvidenceItem(febra, "True", mockNetwork);
            var febraFalse = new EvidenceItem(febra, "False", mockNetwork);

            oboseala.addProbability(new Query(new List<EvidenceItem> { febraFalse }, "False"), 0.8);
            oboseala.addProbability(new Query(new List<EvidenceItem> { febraFalse }, "True"), 0.2);
            
            oboseala.addProbability(new Query(new List<EvidenceItem> { febraTrue }, "False"), 0.4);
            oboseala.addProbability(new Query(new List<EvidenceItem> { febraTrue }, "True"), 0.6);

            var anorexie = mockNetwork.Nodes["anorexie"];

            anorexie.addProbability(new Query(new List<EvidenceItem> { febraFalse }, "False"), 0.9);
            anorexie.addProbability(new Query(new List<EvidenceItem> { febraFalse }, "True"), 0.1);

            anorexie.addProbability(new Query(new List<EvidenceItem> { febraTrue }, "False"), 0.5);
            anorexie.addProbability(new Query(new List<EvidenceItem> { febraTrue }, "True"), 0.5);
        }

        [TestMethod]
        public void TestAskingQuestions()
        {
            var gripa = mockNetwork.getNodeByName("gripa");
            var q = new Query(Enumerable.Empty<EvidenceItem>(), "False");
            Assert.AreEqual(0.9, gripa.ProbabilityDistribution[q], 1e-3);

            var o = new Query(Enumerable.Empty<EvidenceItem>(), "True");
            Assert.AreEqual(0.1, gripa.ProbabilityDistribution[o], 1e-3);
        }
    }
}
