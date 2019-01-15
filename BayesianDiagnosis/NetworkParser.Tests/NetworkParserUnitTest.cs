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
        static string path = "simpleDiseases.xml";
        static Network mockNetwork = NetworkParser.Parser.parse(path);

        [TestMethod]
        public void TestHasAllNodes()
        {
            Assert.AreEqual(5, mockNetwork.Nodes.Count);
        }

        [TestMethod]
        public void TestHasAllLinks()
        {
            var gripa = mockNetwork.Nodes["gripa"];
            var abces = mockNetwork.Nodes["abces"];
            var febra = mockNetwork.Nodes["febra"];
            var anorexie = mockNetwork.Nodes["anorexie"];
            var oboseala = mockNetwork.Nodes["oboseala"];

            Assert.AreEqual(1, gripa.Effects.Count);
            Assert.AreEqual(1, abces.Effects.Count);
            Assert.AreEqual(2, febra.Effects.Count);
            Assert.AreEqual(0, anorexie.Effects.Count);
            Assert.AreEqual(0, anorexie.Effects.Count);

            Assert.AreEqual(0, gripa.Causes.Count);
            Assert.AreEqual(0, abces.Causes.Count);
            Assert.AreEqual(2, febra.Causes.Count);
            Assert.AreEqual(1, anorexie.Causes.Count);
            Assert.AreEqual(1, anorexie.Causes.Count);
        }

        [TestMethod]
        public void TestParsedNetworkAnswersQuestion()
        {
            List<Fact> evidence = new List<Fact> { new Fact("anorexie", "true", mockNetwork), new Fact("oboseala", "true", mockNetwork) };

            Node gripa = mockNetwork.Nodes["gripa"];
            Query question = new Query(gripa, "true", evidence);

            Assert.AreEqual(0.39628, mockNetwork.answer(question), 1e-4);
        }
    }
}
