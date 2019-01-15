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
    public class Parser
    {
        static public Network parse(string path)
        {
            XmlDocument doc = new XmlDocument();

            if (File.Exists(path))
            {
                doc.Load(path);
                return parse(doc);
            }
            else
            {
                return new Network();
            }
        }

        static public Network parse(XmlDocument xmlDocument)
        {
            Network network = new Network();
            XmlNodeList nodeList = xmlDocument.DocumentElement.SelectNodes("/net/node");

            foreach (XmlNode node in nodeList)
            {
                string name = node.SelectSingleNode("name").InnerText;
                var outcomes = node.SelectNodes("outcome").Cast<XmlNode>()
                                    .Select(outcomeNode => outcomeNode.InnerText)
                                    .ToList();


                network.addNode(name, outcomes);

                XmlNodeList probabilityList = node.SelectNodes("table/probability");
                foreach (XmlNode p in probabilityList)
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
