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
            gripa.addProbability(new Query(gripa, "false", Enumerable.Empty<Fact>()), 0.9);
            gripa.addProbability(new Query(gripa, "true", Enumerable.Empty<Fact>()), 0.1);

            var abces = mockNetwork.Nodes["abces"];
            abces.addProbability(new Query(abces, "false", Enumerable.Empty<Fact>()), 0.95);
            abces.addProbability(new Query(abces, "true", Enumerable.Empty<Fact>()), 0.05);

            var febra = mockNetwork.Nodes["febra"];
            var gripaFalse = new Fact(gripa, "false");
            var gripaTrue = new Fact(gripa, "true");
            var abcesFalse = new Fact(abces, "false");
            var abcesTrue = new Fact(abces, "true");

            febra.addProbability(new Query(febra, "false", new List<Fact> { gripaFalse, abcesFalse }), 0.95);
            febra.addProbability(new Query(febra, "true", new List<Fact> { gripaFalse, abcesFalse }), 0.05);

            febra.addProbability(new Query(febra, "false", new List<Fact> { gripaFalse, abcesTrue }), 0.75);
            febra.addProbability(new Query(febra, "true", new List<Fact> { gripaFalse, abcesTrue }), 0.25);

            febra.addProbability(new Query(febra, "false", new List<Fact> { gripaTrue, abcesFalse }), 0.3);
            febra.addProbability(new Query(febra, "true", new List<Fact> { gripaTrue, abcesFalse }), 0.7);

            febra.addProbability(new Query(febra, "false", new List<Fact> { gripaTrue, abcesTrue }), 0.2);
            febra.addProbability(new Query(febra, "true", new List<Fact> { gripaTrue, abcesTrue }), 0.8);

            var oboseala = mockNetwork.Nodes["oboseala"];
            var febraTrue = new Fact(febra, "true");
            var febraFalse = new Fact(febra, "false");

            oboseala.addProbability(new Query(oboseala, "false", new List<Fact> { febraFalse }), 0.8);
            oboseala.addProbability(new Query(oboseala, "true", new List<Fact> { febraFalse }), 0.2);
            
            oboseala.addProbability(new Query(oboseala, "false", new List<Fact> { febraTrue }), 0.4);
            oboseala.addProbability(new Query(oboseala, "true", new List<Fact> { febraTrue }), 0.6);

            var anorexie = mockNetwork.Nodes["anorexie"];

            anorexie.addProbability(new Query(anorexie, "false", new List<Fact> { febraFalse }), 0.9);
            anorexie.addProbability(new Query(anorexie, "true", new List<Fact> { febraFalse }), 0.1);

            anorexie.addProbability(new Query(anorexie, "false", new List<Fact> { febraTrue }), 0.5);
            anorexie.addProbability(new Query(anorexie, "true", new List<Fact> { febraTrue }), 0.5);
        }

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
