using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.LabelEditor.DataModel
{
    class LabelItem : UIObject
    {
        ViewLabel mSource;
        string mName;
        string mEditingName = null;
        EditCallback mEditCallback;
        private LabelEditor mOwner;

        public LabelItem(LabelEditor owner,string name)
        {
            mOwner = owner;
            mName = name;
        }

        public LabelItem(LabelEditor owner, ViewLabel label)
        {
            mOwner = owner;
            mSource = label;
            mName = mSource.Name; 
        }

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                if (mName == value)
                    return;
                mName = value;
                OnPropertyChanged("Name");
            }
        }

        public ViewLabel Source
        {
            get
            {
                return mSource;
            }
        }

        public LabelEditor Owner { get { return mOwner; } }

        public string EditingName
        {
            get
            {
                return mEditingName;
            }
            set
            {
                if (mEditingName == value)
                    return;
                mEditingName = value;
                OnPropertyChanged("EditingName");
            }
        }

        public LabelType LabelType
        {
            get
            {
                return mSource?.Type ?? LabelType.Label;
            }
        }

        public bool CanDelete
        {
            get
            {
                return LabelType == LabelType.Label;
            }
        }

        public bool CanEdit
        {
            get
            {
                return LabelType == LabelType.Label && Source == null;
            }
        }

        public bool IsEditing
        {
            get
            {
                return mEditingName != null;
            }
        }

        public void StartEdit(EditCallback callback)
        {
            if (IsEditing)
                return;
            EditingName = Name;
            mEditCallback = callback;
            OnPropertyChanged("IsEditing");
        }

        public void CancelEdit()
        {
            EditingName = null;
            OnPropertyChanged("IsEditing");
        }

        public void SaveEdit()
        {
            mEditCallback?.Invoke(Name, EditingName);
            Name = EditingName;
            CancelEdit();
        }

        public delegate void EditCallback(string oldname, string newname);
    }
}
