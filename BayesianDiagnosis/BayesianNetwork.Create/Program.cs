using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BayesianNetwork.Create
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Dog-Problem.xml");

            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/BIF/NETWORK/DEFINITION");
            XmlNodeList outcomeList = doc.DocumentElement.SelectNodes("/BIF/NETWORK/DEFINITION/GIVEN");

            string print = null;
            foreach (XmlNode node in nodeList)
            {
                string name = null;
                string property = null;
               
                try
                {
                    name = node.SelectSingleNode("FOR").InnerText;
                    property = node.SelectSingleNode("TABLE").InnerText;

                   print = name + " " + property;
                   foreach (XmlNode outcomeNode in outcomeList)
                   {
                       if (outcomeNode.ParentNode == node)
                       {
                           print +=" " + outcomeNode.InnerText;
                       }
                   }
                   Console.WriteLine(print);
                   print = null;
                }
                catch
                {
                    Console.WriteLine(name + " " + property);
                }
            }
            
        }
    }
}
