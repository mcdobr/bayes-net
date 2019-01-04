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
            nodes.Add(_name, new Node(_name, _domainValues, this));
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

        public Network addLink(Node cause, Node effect)
        {
            cause.addEffect(effect);
            effect.addCause(cause);
            return this;
        }

        public Network addLink(string cause, string effect)
        {
            Node causeNode = getNodeByName(cause);
            Node effectNode = getNodeByName(effect);
            return addLink(causeNode, effectNode);
        }

        public void removeLink(Node cause, Node effect)
        {
            throw new NotImplementedException();
        }

        public void removeLink(string cause, string effect)
        {
            throw new NotImplementedException();
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

        public double answer(Query question)
        {
            var distribution = enumerationAsk(question.Target, question.Facts);
            return distribution[question];
        }

        /**
         * Compute the distribution of the target variable given a collection of facts
         */
        private Dictionary<Query, double> enumerationAsk(Node target, ICollection<Fact> facts)
        {
            var distribution = new Dictionary<Query, double>();
            foreach (var possibleValue in target.DomainValues)
            {
                var nodes = new List<Node>(target.Network.Nodes.Values);
                var newFacts = new List<Fact>(facts);
                newFacts.Add(new Fact(target, possibleValue));

                distribution.Add(new Query(target, possibleValue, facts), enumerateAll(nodes, newFacts));
            }
            
            return normalizeDistribution(distribution);
        }

        /**
         * Compute the probability recursively
         */
        private double enumerateAll(ICollection<Node> nodes, ICollection<Fact> facts)
        {

            if (nodes.Count == 0)
            {
                return 1.0;
            }
            else
            {
                var targetNode = nodes.First();
                // Remove node from node list
                var newNodes = new List<Node>(nodes);
                newNodes.Remove(targetNode);

                bool isTargetNodeSet = facts.Any(fact => fact.Node == targetNode);
                if (isTargetNodeSet)
                {
                    string targetValue = facts.First(fact => fact.Node == targetNode).Value;   
                    // Get parents' values
                    var causalFacts = facts.Where(fact => targetNode.Causes.Any(node => node == fact.Node));
                    // Compute P(targetNode = targetValue | parentsOf(targetNode))
                    double probabilityOfTarget = targetNode.ProbabilityDistribution[new Query(targetNode, targetValue, causalFacts)];

                    return probabilityOfTarget * enumerateAll(newNodes, facts);
                }
                else
                {
                    double probabilityOfTarget = 0.0;
                    foreach (string targetValue in targetNode.DomainValues)
                    {
                        // Get facts for parents
                        var causalFacts = facts.Where(fact => targetNode.Causes.Any(node => node == fact.Node));

                        // Probability if 
                        double probabilityOfTargetCase = targetNode.ProbabilityDistribution[new Query(targetNode, targetValue, causalFacts)];

                        ICollection<Fact> newFacts = new List<Fact>(facts);
                        newFacts.Add(new Fact(targetNode, targetValue));

                        probabilityOfTarget += probabilityOfTargetCase * enumerateAll(newNodes, newFacts);
                    }

                    return probabilityOfTarget;
                }
            }
        }

        private Dictionary<Query, double> normalizeDistribution(Dictionary<Query, double> distribution)
        {
            var normalized = new Dictionary<Query, double>();
            double sumOfOldProbabilities = distribution.Values.Sum();
            double alpha = 1 / sumOfOldProbabilities;

            foreach (var kvp in distribution)
            {
                normalized[kvp.Key] = alpha * kvp.Value;
            }

            return normalized;
        }
        
        public IDictionary<string, Node> Nodes
        {
            get { return nodes; }
        }
    }
}
