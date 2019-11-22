using Hyperwave.UserCache;
using System;

namespace Hyperwave.ViewModel
{
    public interface IDraftMailSource
    {
        Guid DraftId { get; }
        ViewAccount Account { get; set; }
        string Body { get; set; }
        bool IsReply { get; set; }
        EntityInfo[] Recipients { get; set; }
        string Subject { get; set; }

        void NotifySaved();
    }
}