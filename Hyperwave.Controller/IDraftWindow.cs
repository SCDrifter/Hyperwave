using Hyperwave.ViewModel;

namespace Hyperwave.Controller
{
    public interface IDraftWindow
    {
        void SetDraft(DraftMessageSource mail);
        void SetFocus();
        void DraftDeleted();
        void Save();
    }
}
