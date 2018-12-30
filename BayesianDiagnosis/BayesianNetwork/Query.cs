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
        private readonly ICollection<EvidenceItem> evidence;
        private readonly string value;

        public Query(IEnumerable<EvidenceItem> _evidence, string _value)
        {
            evidence = new HashSet<EvidenceItem>(_evidence);
            value = _value;
        }

        public Query addEvidenceItem(EvidenceItem item)
        {
            ICollection<EvidenceItem> newEvidence = new HashSet<EvidenceItem>(this.Evidence);
            newEvidence.Add(item);
            return new Query(newEvidence, Value);
        }

        public ICollection<EvidenceItem> Evidence
        {
            get
            {
                return evidence;
            }
        }

        public string Value
        {
            get
            {
                return value;
            }
        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Query other = obj as Query;

            return (new HashSet<EvidenceItem>(Evidence)).SetEquals(other.Evidence) && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}/{1}", Evidence, Value).GetHashCode();
        }
    }
}
