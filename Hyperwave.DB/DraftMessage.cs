using Hyperwave.UserCache;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.DB
{
    public class DraftMessage: IComparable<DraftMessage>
    {
        

        public DraftMessage()
        {
            DraftId = Guid.NewGuid();
        }

        internal DraftMessage(Guid id)
        {
            DraftId = id;
        }



        internal AccountDatabase DB { get; set; }
        
        public long AccountId { get; set; }
        public Guid DraftId { get; private set; }
        public string Body { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsReply { get; set; }
        public SortedSet<EntityInfo> Recipients { get; private set; } = new SortedSet<EntityInfo>();
        public string Subject { get; set; }

        int IComparable<DraftMessage>.CompareTo(DraftMessage other)
        {
            return DraftId.CompareTo(other.DraftId);
        }
    }
}
