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
            nodes.Add(_name, new Node(_name, _domainValues.Select(str => str.Trim().ToLower()), this));
            return this;
        }

        public Network addNode(string _name)
        {
            return addNode(_name, new[] { "true", "false" });
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
            //var nodes = new List<Node>(target.Network.Nodes.Values);
            var nodes = topologicalSort(target.Network.Nodes.Values);


            foreach (var possibleValue in target.DomainValues)
            {
                var newFacts = new List<Fact>(facts);
                newFacts.Add(new Fact(target, possibleValue));

                distribution.Add(new Query(target, possibleValue, facts), enumerateAll(nodes, newFacts));
            }
            
            return normalizeDistribution(distribution);
        }

        /**
         * Computes the probability of the given facts by recursing on all the states that include the supplied state
         */
        private double enumerateAll(ICollection<Node> nodes, ICollection<Fact> facts)
        {

            if (nodes.Count == 0)
            {
                return 1.0;
            }
            else
            {
                var head = nodes.First();

                var nodesTail = new List<Node>(nodes);
                nodesTail.Remove(head);

                bool isHeadSet = facts.Any(fact => fact.Node == head);
                if (isHeadSet)
                {
                    string headValue = facts.First(fact => fact.Node == head).Value;
                    var causalFacts = facts.Where(fact => head.Causes.Any(node => node == fact.Node));
                    double probabilityOfTarget = head.ProbabilityDistribution[new Query(head, headValue, causalFacts)];

                    return probabilityOfTarget * enumerateAll(nodesTail, facts);
                }
                else
                {
                    double probabilityOfTarget = 0.0;
                    foreach (string headValue in head.DomainValues)
                    {
                        var causalFacts = facts.Where(fact => head.Causes.Any(node => node == fact.Node));
                        double probabilityOfTargetCase = head.ProbabilityDistribution[new Query(head, headValue, causalFacts)];

                        ICollection<Fact> newFacts = new List<Fact>(facts);
                        newFacts.Add(new Fact(head, headValue));

                        probabilityOfTarget += probabilityOfTargetCase * enumerateAll(nodesTail, newFacts);
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

        private ICollection<Node> topologicalSort(ICollection<Node> nodes)
        {
            var stack = new Stack<Node>();
            var isVisitedAlready = new Dictionary<Node, bool>();

            var sortedNodes = new List<Node>();
            foreach (var startNode in nodes)
            {
                if (!isVisitedAlready.ContainsKey(startNode))
                {
                    stack.Push(startNode);
                    isVisitedAlready[startNode] = true;

                    while (stack.Count > 0)
                    {
                        var topNode = stack.Peek();

                        int visitedEffects = 0;
                        foreach (var effect in topNode.Effects)
                        {
                            if (!isVisitedAlready.ContainsKey(effect))
                            {
                                stack.Push(effect);
                                isVisitedAlready[effect] = true;
                            }
                            else
                                ++visitedEffects;
                        }

                        bool shouldPop = (visitedEffects == topNode.Effects.Count);
                        if (shouldPop)
                            sortedNodes.Add(stack.Pop());
                    }
                }
            }

            sortedNodes.Reverse();
            return sortedNodes;
        }


        public IDictionary<string, Node> Nodes
        {
            get { return nodes; }
        }
    }
}
