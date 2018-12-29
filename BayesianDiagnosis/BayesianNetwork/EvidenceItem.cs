using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNetwork
{
    /**
     * Class representing a single item of evidence (gripa = true) 
     */
    public class EvidenceItem
    {
        private readonly Node node;
        private readonly string value;
        private readonly Network network;

        public EvidenceItem(Node _node, string _value, Network _network)
        {
            node = _node;
            value = _value;
            network = _network;
        }

        public EvidenceItem(string _nodeName, string _value, Network _network) : this(_network.getNodeByName(_nodeName), _value, _network)
        { }

        public Node Node
        {
            get { return node; }
        }

        public string Value
        {
            get { return value; }
        }
    }
}
