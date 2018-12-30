using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianNetwork
{
    public class Evidence
    {
        private readonly ICollection<EvidenceItem> items;

        public ICollection<EvidenceItem> Items
        {
            get
            {
                return items;
            }
        }

        public Evidence(IEnumerable<EvidenceItem> _items)
        {
            items = new HashSet<EvidenceItem>();
            items.Union(_items);
        }

        public Evidence() : this(Enumerable.Empty<EvidenceItem>())
        {
        }

        public Evidence addEvidenceItem(EvidenceItem newItem)
        {
            Evidence result = new Evidence(this.Items.Union(new[] { newItem }));
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Evidence other = obj as Evidence;
            return (new HashSet<EvidenceItem>(Items)).SetEquals(other.Items);
        }

        public static bool operator ==(Evidence e, Evidence o)
        {
            return e.Equals(o);
        }

        public static bool operator !=(Evidence e, Evidence o)
        {
            return !(e == o);
        }

        public override int GetHashCode()
        {
            return string.Format("{0}", items).GetHashCode();
        }
    }
}
