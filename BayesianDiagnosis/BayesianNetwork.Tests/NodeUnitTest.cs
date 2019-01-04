using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BayesianNetwork.Tests
{
    [TestClass]
    public class NodeUnitTest
    {
        [TestMethod]
        public void TestAddingEdgesToNodes()
        {
            Network mockNetwork = new Network();
            Node cause = new Node("cauza", new[] { "True", "False" }, mockNetwork);
            Node effect = new Node("efect", new[] { "True", "False" }, mockNetwork);

            cause.addEffect(effect);
            Assert.AreEqual(1, cause.Effects.Count);

            effect.addCause(cause);
            Assert.AreEqual(1, effect.Causes.Count);
        }
    }
}
