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
            Node cause = new Node("cauza", new[] { "True", "False" });
            Node effect = new Node("efect", new[] { "True", "False" });

            cause.addEffect(effect);
            Assert.AreEqual(1, cause.Effects.Count);

            effect.addCause(cause);
            Assert.AreEqual(1, effect.Causes.Count);
        }
    }
}
