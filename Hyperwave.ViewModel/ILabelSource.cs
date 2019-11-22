namespace Hyperwave.ViewModel
{
    public interface ILabelSource
    {
        int Id { get; }
        LabelType Type { get; }
        string Name { get; }
        int UnreadCount { get; }
        bool IsVirtual { get; }
    }

    public enum LabelType
    {
        Label,
        Inbox,
        Outbox,
        Drafts,
        AllianceMail,
        CorpMail,
        MailingList
    }
}