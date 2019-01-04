using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNetwork
{
    /**
     * Class representing a single fact (gripa = true) 
     */
    public class Fact
    {
        private readonly Node node;
        private readonly string value;

        public Fact(Node _node, string _value)
        {
            node = _node;
            value = _value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Fact other = obj as Fact;
            return Node == other.Node && value == other.Value;
        }

        public override int GetHashCode()
        {
            return string.Format("{0};{1};", Node, Value).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", node, value);
        }

        public Fact(string _nodeName, string _value, Network _network) : this(_network.getNodeByName(_nodeName), _value)
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
