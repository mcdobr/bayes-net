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
        private String name;
        private HashSet<String> domainValues;
        private ICollection<Node> causes;
        private ICollection<Node> effects;

        public Node(String _name, IEnumerable<String> _domainValues)
        {
            name = _name;

            _domainValues = _domainValues.Select(value => value.Trim());

            domainValues = new HashSet<string>();
            domainValues.UnionWith(_domainValues);


            causes = new HashSet<Node>();
            effects = new HashSet<Node>();
        }

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

        public ICollection<Node> Causes
        {
            get { return causes; }
        }

        public ICollection<Node> Effects
        {
            get { return effects; }
        }
    }
}
