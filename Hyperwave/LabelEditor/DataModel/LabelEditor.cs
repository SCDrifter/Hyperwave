using Hyperwave.Controller;
using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.LabelEditor.DataModel
{
    class LabelEditor : UIObject
    {
        ObservableCollection<LabelItem> mLabelItems = new ObservableCollection<LabelItem>();
        List<UndoItem> mUndoHistory = new List<UndoItem>();
        int mUndoIndex = 0;

        ViewAccount mAccount;
        private LabelItem mCancelItem;
        private bool mCanEdit = false;
        decimal mMaxProgress = 10; decimal mCurrentProgress = 5;
        NewItem mNewItem = new NewItem();

        public LabelEditor()
        {

            mLabelItems.Add(new LabelItem(this,"Label 1"));
            mLabelItems.Add(new LabelItem(this,"Label 2"));
            mLabelItems.Add(new LabelItem(this,"Label 3"));
            //mLabelItems[2].StartEdit(null);
            
        }

        public void Load(ViewAccount account)
        {
            mUndoHistory.Clear();
            UndoIndex = 0;            
            mLabelItems.Clear();
            mAccount = account;
            foreach (ViewLabel i in account.Labels)
            {
                if (i.Type == LabelType.MailingList)
                    continue;

                mLabelItems.Add(new LabelItem(this,i));
            }
            CanEdit = true;
        }

        public ObservableCollection<LabelItem> LabelItems
        {
            get { return mLabelItems; }
        }

        void Item_IsEditingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (mCancelItem != sender || mCancelItem.IsEditing)
                return;

            mCancelItem.UnregisterHandler("IsEditing", Item_IsEditingChanged);
            mCancelItem = null;

        }

        public NewItem NewItem
        {
            get { return mNewItem; }
        }

        private int UndoIndex
        {
            get { return mUndoIndex; }
            set
            {
                if (mUndoIndex == value)
                    return;
                mUndoIndex = value;
                OnPropertyChanged("CanRedo");
                OnPropertyChanged("CanUndo");
            }
        }

        void AddUndoItem(UndoItem item)
        {
            ClearRedoHistory();
            mUndoHistory.Insert(UndoIndex++, item);
        }

        private void ClearRedoHistory()
        {
            while (CanRedo)
                mUndoHistory.RemoveAt(UndoIndex);
        }

        public void EditItem(LabelItem item)
        {
            CancelEdit();

            mCancelItem = item;

            mCancelItem.StartEdit((oname, nname) => AddUndoItem(new EditUndoItem(item, oname, nname)));

            mCancelItem.RegisterHandler("IsEditing", Item_IsEditingChanged);
        }

        internal void DeleteItem(LabelItem item)
        {
            CancelEdit();
            RemoveUndoItem uitem = new RemoveUndoItem(item, mLabelItems.IndexOf(item));
            AddUndoItem(uitem);
            uitem.Redo(LabelItems);
        }

        public void CancelEdit()
        {
            mCancelItem?.CancelEdit();
        }

        public void AddItem(string text)
        {
            CancelEdit();
            var item = new LabelItem(this,text);
            mLabelItems.Add(item);
            AddUndoItem(new AddUndoItem(item, LabelItems.IndexOf(item)));
        }

        public bool CanUndo
        {
            get
            {
                return CanEdit && mUndoIndex > 0;
            }
        }
        public bool CanRedo
        {
            get
            {
                return CanEdit && mUndoIndex < mUndoHistory.Count;
            }
        }

        public void Undo()
        {
            if (!CanUndo)
                return;

            CancelEdit();

            mUndoHistory[--UndoIndex].Undo(LabelItems);
        }

        public void Redo()
        {
            if (!CanRedo)
                return;

            CancelEdit();

            mUndoHistory[UndoIndex++].Redo(LabelItems);
        }

        public async Task Apply()
        {
            CancelEdit();

            List<LabelItem> add = new List<LabelItem>();
            List<LabelItem> remove = new List<LabelItem>();
            GetWorkingItems(add, remove);

            CurrentProgress = 0;
            MaxProgress = add.Count + remove.Count;

            CanEdit = false;

            foreach(var i in remove)
            {
                if (i.Source == null)
                    continue;
                await App.CurrentClient.RemoveLabelWait(((ISourceInfo)i.Source).LabelSource);
                CurrentProgress++;
            }

            foreach(var i in add)
            {
                await App.CurrentClient.AddLabelWait(((ISourceInfo)mAccount).AccountSource, i.Name);
                CurrentProgress++;
            }

            //CanEdit = true;
            //Load(mAccount);
        }

        private void GetWorkingItems(List<LabelItem> add, List<LabelItem> remove)
        {
            ClearRedoHistory();

            foreach(var i in mUndoHistory)
            {
                AddRemoveUndoItem item = i as AddRemoveUndoItem;
                switch(i.Type)
                {
                    case UndoType.Add:
                        add.Add(item.Item);
                        break;
                    case UndoType.Delete:
                        if(item.Item.Source != null)
                            remove.Add(item.Item);
                        add.Remove(item.Item);
                        break;
                }
            }
        }

        public bool CanEdit
        {
            get { return mCanEdit; }
            set
            {
                if (value == mCanEdit)
                    return;
                mCanEdit = value;
                NewItem.CanEdit = value;
                OnPropertyChanged("CanEdit");
                OnPropertyChanged("CanUndo");
                OnPropertyChanged("CanRedo");
                OnPropertyChanged("IsWorking");
            }
        }

        public bool IsWorking
        {
            get
            {
                return !CanEdit;
            }
        }

        public decimal MaxProgress
        {
            get { return mMaxProgress; }
            private set
            {
                if (mMaxProgress == value)
                    return;
                mMaxProgress = value;
                OnPropertyChanged("MaxProgress");
            }
        }


        public decimal CurrentProgress
        {
            get { return mCurrentProgress; }
            private set
            {
                if (mCurrentProgress == value)
                    return;
                mCurrentProgress = value;
                OnPropertyChanged("CurrentProgress");
            }
        }
    }
}
