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
        private readonly Evidence evidence;
        private readonly string value;

        public Query(Evidence _evidence, string _value)
        {
            evidence = _evidence;
            value = _value;
        }

        public Query addEvidenceItem(EvidenceItem item)
        {
            Evidence newEvidence = new Evidence();
            newEvidence.UnionWith(this.evidence);
            newEvidence.Add(item);

            return new Query(newEvidence, value);
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
    }
}
