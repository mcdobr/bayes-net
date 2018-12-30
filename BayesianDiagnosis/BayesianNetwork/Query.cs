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
        private readonly Node target;
        private readonly Evidence evidence;
        private readonly string value;

        public Query(Node _target, Evidence _evidence, string _value)
        {
            target = _target;
            evidence = _evidence;
            value = _value;
        }

        public Query addEvidenceItem(EvidenceItem item)
        {
            Evidence newEvidence = this.evidence.addEvidenceItem(item);
            return new Query(Target, newEvidence, Value);
        }

        public Node Target
        {
            get
            {
                return target;
            }
        }

        public Evidence Evidence
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

            return Target == other.Target && Evidence == other.Evidence && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}/{1}/{2}", Target, Evidence, Value).GetHashCode();
        }
    }
}
