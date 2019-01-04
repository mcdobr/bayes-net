using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNetwork
{
    public class Node
    {
        private string name;
        private readonly HashSet<string> domainValues;
        private ICollection<Node> causes;
        private ICollection<Node> effects;
        private IDictionary<Query, double> probabilityDistribution;
        private Network network;

        public Node(string _name, IEnumerable<string> _domainValues, Network _network)
        {
            name = _name;

            _domainValues = _domainValues.Select(value => value.Trim());

            domainValues = new HashSet<string>();
            domainValues.UnionWith(_domainValues);


            causes = new HashSet<Node>();
            effects = new HashSet<Node>();

            probabilityDistribution = new Dictionary<Query, double>();
            network = _network;
        }

        public Node(string _name, Network _network) : this(_name, new[] { "True", "False" }, _network) { }

        public void addCause(Node cause)
        {
            causes.Add(cause);
        }

        public bool removeCause(Node cause)
        {
            return causes.Remove(cause);
        }

        public void addEffect(Node effect)
        {
            effects.Add(effect);
        }

        public bool removeEffect(Node effect)
        {
            return effects.Remove(effect);
        }

        public void addProbability(Query query, double value)
        {
            probabilityDistribution.Add(query, value);
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return name;
        }

        public ICollection<Node> Causes
        {
            get { return causes; }
        }

        public ICollection<Node> Effects
        {
            get { return effects; }
        }

        public ICollection<string> DomainValues
        {
            get { return domainValues; }   
        }

        public IDictionary<Query, double> ProbabilityDistribution
        {
            get { return probabilityDistribution; }
        }

        public Network Network
        {
            get
            { return network; }
        }
    }
}
