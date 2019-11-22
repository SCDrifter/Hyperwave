using Hyperwave.UserCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class DraftMailRecipient : MailRecipient
    {
        private ReciepientEditor mEditor = new ReciepientEditor();
        private bool mIsEditing = false;

        public DraftMailRecipient()
        {
            mEditor.Selected += mEditor_Selected;
        }

        public DraftMailRecipient(EntityInfo info)
            :this()
        {
            SelectItem(info);
        }

        private async void mEditor_Selected(object sender, SelectedSuggestionEventArgs e)
        {
            if (e.InfoSelected == null)
            {
                EntityInfo[] list = await EntityLookupAsync.Search(e.TextSelected, SearchOptions.Offline | SearchOptions.ExactMatch);

                if (list.Length == 1)
                    e.InfoSelected = list[0];
                else
                {
                    e.InfoSelected = new EntityInfo()
                    {
                        EntityID = -1,
                        EntityType = Common.EntityType.Character,
                        Name = e.TextSelected
                    };
                }

                e.TextSelected = null;
            }
            SelectItem(e.InfoSelected);
        }

        protected virtual void SelectItem(EntityInfo einfo)
        {
            Id = einfo.EntityID;
            Type = einfo.EntityType;
            Name = einfo.Name;
            ImageUrl16 = einfo.ImageUrl16;
            ImageUrl32 = einfo.ImageUrl32;
            ImageUrl64 = einfo.ImageUrl64;
            ImageUrl128 = einfo.ImageUrl128;
            CancelEdit();
        }

        internal EntityInfo ToEntityInfo()
        {
            return new EntityInfo()
            {
                EntityType = Common.EntityType.Character,
                EntityID = Id,
                Name = Name
            };
        }

        public ReciepientEditor Editor { get { return mEditor; } }

        public override long Id
        {
            get
            {
                return base.Id;
            }

            set
            {
                if (base.Id == value)
                    return;

                base.Id = value;
                OnPropertyChanged("IsValid");
            }
        }

        public void StartEdit()
        {
            mEditor.StartEdit(Name);
            IsEditing = true;
        }

        public void CancelEdit()
        {
            mEditor.Text = "";
            IsEditing = false;
        }

        public virtual bool IsNewPlaceholder
        {
            get { return false; }
        }

        public bool IsEditing
        {
            get { return mIsEditing; }
            protected set
            {
                if (mIsEditing == value)
                    return;
                mIsEditing = value;
                OnPropertyChanged("IsEditing");
            }
        }

        public bool IsValid
        {
            get
            {
                return Id != -1 && Id != 0;
            }
        }
    }

    public class NewDraftMailRecipient : DraftMailRecipient
    {
        public NewDraftMailRecipient()
        {
            //Name = "+";
        }
        public override bool IsNewPlaceholder
        {
            get
            {
                return true;
            }
        }

        protected override void SelectItem(EntityInfo einfo)
        {
            DraftMailRecipient recipient = new DraftMailRecipient(einfo);

            if (NewReceipient != null)
                NewReceipient(this, new NewReceipientEventArgs() { Recipient = recipient });
            CancelEdit();
            StartEdit();
        }

        public event EventHandler<NewReceipientEventArgs> NewReceipient;
    }

    public class NewReceipientEventArgs : EventArgs
    {
        public DraftMailRecipient Recipient { get; set; }
    }
}
