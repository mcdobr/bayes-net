using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using BayesianNetwork;


namespace NetworkParser
{
    public class NetworkCreate
    {
        static Network network;

        string path = "C:\\Users\\Iulius\\Desktop\\bayes-net-master\\BayesianDiagnosis\\network.xml";

        public Network startParsing()
        {
            XmlDocument doc = new XmlDocument();
            network = new Network();

            if (File.Exists(path))
                doc.Load(path);
            else
                return network;

            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/net/node");

            foreach (XmlNode node in nodeList)
            {
                string name = node.SelectSingleNode("name").InnerText;
                network.addNode(name);
                
                XmlNodeList probabiliyList = node.SelectNodes("table/probability");
                foreach (XmlNode p in probabiliyList)
                {
                    var given = p.SelectSingleNode("given");

                    if (given.ChildNodes.Count != 0)
                    {
                        XmlNodeList factsList = given.SelectNodes("fact");
                        Console.WriteLine("{0}", factsList.Count);

                        IDictionary<List<Fact>, string> allFacts = new Dictionary<List<Fact>, string>();
                        List<Fact> list = new List<Fact>();

                        string externalOutcome = p.SelectSingleNode("outcome").InnerText;
                        double prob = Double.Parse(p.SelectSingleNode("value").InnerText);


                        foreach (XmlNode f in factsList)
                        {
                            string parent = f.SelectSingleNode("parent").InnerText;
                            string internalOutcome = f.SelectSingleNode("outcome").InnerText;

                            list.Add(new Fact(network.getNodeByName(parent), internalOutcome));

                            network.addLink(parent, name);
                        }
                        allFacts.Add(list, name);
                        foreach (KeyValuePair<List<Fact>, string> kvp in allFacts)
                        {
                            if (kvp.Value == name)
                            {
                                var disease = network.getNodeByName(name);
                                disease.addProbability(new Query(disease, externalOutcome, kvp.Key), prob);
                            }
                        }
                    }
                    else
                    {
                        var disease = network.getNodeByName(name);
                        string outcome = p.SelectSingleNode("outcome").InnerText;
                        double prob = Double.Parse(p.SelectSingleNode("value").InnerText);
                        Console.WriteLine("P({0} = {1}) = {2}", disease, outcome, prob);
                        disease.addProbability(new Query(disease, outcome, Enumerable.Empty<Fact>()), prob);
                    }
                }
            }
            return network;
        }
    }
}
