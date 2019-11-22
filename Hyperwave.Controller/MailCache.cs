using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Controller
{
    class MailCache
    {
        public List<ViewMailItem> MailItems { get; private set; } = new List<ViewMailItem>();
        public DateTime Expires { get; set; }

        public bool IsValid
        {
            get
            {
                if (Expires > DateTime.Now)
                    return true;
                MailItems.Clear();
                return false;
            }
        }

        public void UpdateCache()
        {
            var items = from a in MailItems where a.OperationFlags.HasFlag(MailOperationFlags.Removing) select a;
            foreach(var i in items)
            {
                MailItems.Remove(i);
            }
        }

        public void Reset()
        {
            MailItems.Clear();
            Expires = DateTime.MinValue;
        }

        public void Set(IEnumerable<ViewMailItem> items)
        {
            MailItems.Clear();
            MailItems.AddRange(items);
            Expires = DateTime.Now.AddSeconds(30);
        }

        public void Add(IEnumerable<ViewMailItem> items)
        {
            MailItems.AddRange(items);
            Expires = DateTime.Now.AddSeconds(30);
        }
    }
}
