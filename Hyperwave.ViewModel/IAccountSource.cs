namespace Hyperwave.ViewModel
{
    public interface IAccountSource
    {
        long Id { get; }
        AccountState AccountState { get; }
        string ImageUrl { get; }
        int UnreadCount { get; set; }
        string UserName { get; }
        ILabelSource[] Labels { get; }
        bool IsExpanded { get; }
    }
}