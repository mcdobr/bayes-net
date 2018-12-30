using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNetwork
{
    /**
     * Class that represents the actual bayesian network
     */
    public class Network
    {
        private IDictionary<string, Node> nodes;

        public Network()
        {
            nodes = new Dictionary<string, Node>();
        }

        public Network addNode(string _name, IEnumerable<string> _domainValues)
        {
            nodes.Add(_name, new Node(_name, _domainValues));
            return this;
        }

        public Network addNode(string _name)
        {
            return addNode(_name, new[] { "True", "False" });
        }

        public void removeNode(Node node)
        {
            throw new NotImplementedException();
        }

        public void removeNode(string node)
        {
            throw new NotImplementedException();
        }

        public void addLink(Node cause, Node effect)
        {
            cause.addEffect(effect);
            effect.addCause(cause);
        }

        public void addLink(string cause, string effect)
        {
            Node causeNode = getNodeByName(cause);
            Node effectNode = getNodeByName(effect);
            addLink(causeNode, effectNode);
        }

        public void removeLink(Node cause, Node effect)
        {
            throw new NotImplementedException();
        }

        public void removeLink(string cause, string effect)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, Node> Nodes
        {
            get { return nodes; }
        }

        public Node getNodeByName(string nodeName)
        {
            Node requestedNode;
            bool found = nodes.TryGetValue(nodeName, out requestedNode);
            if (found)
                return requestedNode;
            else
                throw new KeyNotFoundException();
        }
    }
}
