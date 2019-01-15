using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNetwork
{
    /**
     * Class representing the full query, e.g: P(Febra = True | Gripa = True & Abces = False)
     */
    public class Query
    {
        private readonly ICollection<Fact> facts;
        private readonly Node target;
        private readonly string value;

        public Query(Node _target, string _value, IEnumerable<Fact> _facts)
        {
            target = _target;
            value = _value.Trim().ToLower();
            facts = new HashSet<Fact>(_facts);
        }

        public Query addFact(Fact item)
        {
            ICollection<Fact> newEvidence = new HashSet<Fact>(this.Facts);
            newEvidence.Add(item);
            return new Query(Target, Value, newEvidence);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Query other = obj as Query;

            return (new HashSet<Fact>(Facts)).SetEquals(other.Facts) && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}/{1}/{2}", Target, Value, Facts).GetHashCode();
        }
        
        public ICollection<Fact> Facts
        {
            get
            {
                return facts;
            }
        }

        public Node Target
        {
            get { return target; }
        }

        public string Value
        {
            get
            {
                return value;
            }
        }
    }
}
