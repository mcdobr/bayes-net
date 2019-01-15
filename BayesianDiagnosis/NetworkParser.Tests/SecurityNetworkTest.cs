using BayesianNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Tests
{
    [TestClass]
    public class SecurityNetworkUnitTest
    {
        static string path = "security.xml";
        static Network mockNetwork = NetworkParser.Parser.parse("security.xml");


        /**
         * XML cu graful din Artificial Intelligence, A Modern Approach (ed a 3-a) de Stuart Russell și Peter Norvig
         */
        [TestMethod]
        public void TestAnsweringSecurityNetworkQuestionsCorrectly()
        {
            var b = mockNetwork.Nodes["burglary"];
            var j = mockNetwork.Nodes["john calls"];
            var m = mockNetwork.Nodes["Mary calls"];

            List<Fact> evidence = new List<Fact> { new Fact(j, "true"), new Fact(m, "true") };
            
            double probFalse = mockNetwork.answer(new Query(b, "false", evidence));
            Assert.AreEqual(0.716, probFalse, 1e-3);

            double probTrue = mockNetwork.answer(new Query(b, "true", evidence));
            Assert.AreEqual(0.284, probTrue, 1e-3);

        }
    }
}
