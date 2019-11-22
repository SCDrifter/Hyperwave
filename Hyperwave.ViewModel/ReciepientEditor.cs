using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyperwave.UserCache;
using System.Collections.ObjectModel;
using System.Threading;
using Hyperwave.Common;

namespace Hyperwave.ViewModel
{
    public class ReciepientEditor : UIObject
    {
        ObservableCollection<EntityInfo> mCharacters = new ObservableCollection<EntityInfo>();
        ObservableCollection<EntityInfo> mCorporations = new ObservableCollection<EntityInfo>();
        ObservableCollection<EntityInfo> mAlliances = new ObservableCollection<EntityInfo>();
        ObservableCollection<EntityInfo> mMailinglists = new ObservableCollection<EntityInfo>();
        ObservableCollection<EntityInfo> mItems = new ObservableCollection<EntityInfo>();

        EntityInfo[] mFirstCharacters = null;
        EntityInfo[] mFirstCorporations = null;
        EntityInfo[] mFirstAlliances = null;
        EntityInfo[] mFirstMailinglists = null;

        string mText = "";
        bool mTextModified = false;

        bool mIsLoading = false;

        Task mRunningTask = null;
        CancellationTokenSource mCancelRunningTask = null;

        const int MAX_ITEMS = 10;

        public ReciepientEditor()
        {
            mCharacters.CollectionChanged += Characters_CollectionChanged;
            mCorporations.CollectionChanged += Corporations_CollectionChanged; 
            mAlliances.CollectionChanged += Alliances_CollectionChanged;
            mMailinglists.CollectionChanged += Mailinglists_CollectionChanged;
            mItems.CollectionChanged += Items_CollectionChanged;
        }
        

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasItems");
        }

        private void Mailinglists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasMailinglists");
            mFirstMailinglists = null;
            OnPropertyChanged("FirstMailinglists");
            OnPropertyChanged("HasMoreMailinglists");
        }

        private void Alliances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasAlliances");
            mFirstAlliances = null;
            OnPropertyChanged("FirstAlliances");
            OnPropertyChanged("HasMoreAlliances");
        }

        private void Corporations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasCorporations");
            mFirstCorporations = null;
            OnPropertyChanged("FirstCorporations");
            OnPropertyChanged("HasMoreCorporations");
        }

        private void Characters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasCharacters");
            mFirstCharacters = null;
            OnPropertyChanged("FirstCharacters");
            OnPropertyChanged("HasMoreCharacters");
        }

        public string Text
        {
            get { return mText; }
            set
            {
                if (value == mText)
                    return;

                mText = value;
                mTextModified = true;
                OnPropertyChanged("Text");
                OnPropertyChanged("CanGoOnline");
                OnPropertyChanged("ShowSuggestions");
                
                if (mRunningTask != null && !mRunningTask.IsCompleted)
                {
                    if (mCancelRunningTask == null)
                        return;
                    else
                    {
                        mCancelRunningTask.Cancel();
                        mCancelRunningTask = null;
                    }
                    mRunningTask = null;
                }                

                mCancelRunningTask = new CancellationTokenSource();
                mRunningTask = LoadOffline();
            }
        }

        public void StartEdit(string text)
        {
            mText = text;
            mTextModified = false;

            OnPropertyChanged("Text");
            OnPropertyChanged("CanGoOnline");
            OnPropertyChanged("ShowSuggestions");
        }

        private async Task LoadOffline()
        {
            mAlliances.Clear();
            mCharacters.Clear();
            mCorporations.Clear();
            mMailinglists.Clear();

            if(mText == null)
            {
                mItems.Clear();
                mCancelRunningTask = null;
                return;
            }

            //IsLoading = true;

            var result = await EntityLookupAsync.Search(mText, SearchOptions.Offline, mCancelRunningTask.Token);

            //IsLoading = false;

            LoadResults(result);

            mCancelRunningTask = null;
        }

        public async Task SearchOnline()
        {
            mAlliances.Clear();
            mCharacters.Clear();
            mCorporations.Clear();
            mMailinglists.Clear();

            if (mText == null)
            {
                mItems.Clear();
                mCancelRunningTask = null;
                return;
            }

            IsLoading = true;

            Task<EntityInfo[]> task;
            mRunningTask = task = EntityLookupAsync.Search(mText, SearchOptions.Online);

            var result = await task;

            IsLoading = false;

            LoadResults(result);
            
            
        }

        private void LoadResults(EntityInfo[] result)
        {
            if (result == null)
                return;

            using (DeferUIObjectChanges changes = new DeferUIObjectChanges(this))
            {
                mItems.Clear();
                foreach (var i in result)
                {
                    Items.Add(i);
                    switch (i.EntityType)
                    {
                        case EntityType.Character:
                            mCharacters.Add(i);
                            break;
                        case EntityType.Corporation:
                            mCorporations.Add(i);
                            break;
                        case EntityType.Alliance:
                            mAlliances.Add(i);
                            break;
                        case EntityType.Mailinglist:
                            mMailinglists.Add(i);
                            break;

                    }
                }
            }
        }

        public bool IsLoading
        {
            get { return mIsLoading; }
            private set
            {
                if (mIsLoading == value)
                    return;
                mIsLoading = value;
                OnPropertyChanged("IsLoading");
                OnPropertyChanged("CanGoOnline");
            }
        }

        public ObservableCollection<EntityInfo> Characters
        {
            get { return mCharacters; }
        }

        public EntityInfo[] FirstCharacters
        {
            get
            {
                GetFirstItems(ref mFirstCharacters, mCharacters);

                return mFirstCharacters;
            }
        }

        public bool HasMoreCharacters
        {
            get
            {
                if(mFirstCharacters != null)
                    return mFirstCharacters.Length < mCharacters.Count;
                else
                    return GetFirstItemsCount(mCharacters) < mCharacters.Count;
            }
        }

        public void GetMoreCharacters()
        {
            GetNextItems(ref mFirstCharacters, mCharacters);
            OnPropertyChanged("FirstCharacters");
            OnPropertyChanged("HasMoreCharacters");
        }

        public bool HasCharacters
        {
            get
            {
                return mCharacters.Count > 0;
            }
        }

        public ObservableCollection<EntityInfo> Alliances
        {
            get { return mAlliances; }
        }

        public EntityInfo[] FirstAlliances
        {
            get
            {
                GetFirstItems(ref mFirstAlliances, mAlliances);

                return mFirstAlliances;
            }
        }

        public bool HasAlliances
        {
            get
            {
                return mAlliances.Count > 0;
            }
        }

        public bool HasMoreAlliances
        {
            get
            {
                if (mFirstAlliances != null)
                    return mFirstAlliances.Length < mAlliances.Count;
                else
                    return GetFirstItemsCount(mAlliances) < mAlliances.Count;
            }
        }

        public void GetMoreAlliances()
        {
            GetNextItems(ref mFirstAlliances, mAlliances);
            OnPropertyChanged("FirstAlliances");
            OnPropertyChanged("HasMoreAlliances");
        }

        public ObservableCollection<EntityInfo> Corporations
        {
            get { return mCorporations; }
        }

        public EntityInfo[] FirstCorporations
        {
            get
            {
                GetFirstItems(ref mFirstCorporations, mCorporations);

                return mFirstCorporations;
            }
        }

        public bool HasCorporations
        {
            get
            {
                return mCorporations.Count > 0;
            }
        }

        public bool HasMoreCorporations
        {
            get
            {
                if (mFirstCorporations != null)
                    return mFirstCorporations.Length < mCorporations.Count;
                else
                    return GetFirstItemsCount(mCorporations) < mCorporations.Count;
            }
        }

        public void GetMoreCorporations()
        {
            GetNextItems(ref mFirstCorporations, mCorporations);
            OnPropertyChanged("FirstCorporations");
            OnPropertyChanged("HasMoreCorporations");
        }

        public ObservableCollection<EntityInfo> Mailinglists
        {
            get { return mMailinglists; }
        }

        public EntityInfo[] FirstMailinglists
        {
            get
            {
                GetFirstItems(ref mFirstMailinglists, mMailinglists);

                return mFirstMailinglists;
            }
        }

        public bool HasMailinglists
        {
            get
            {
                return mMailinglists.Count > 0;
            }
        }

        public bool HasMoreMailinglists
        {
            get
            {
                if (mFirstMailinglists != null)
                    return mFirstMailinglists.Length < mMailinglists.Count;
                else
                    return GetFirstItemsCount(mMailinglists) < mMailinglists.Count;
            }
        }

        public void GetMoreMailinglists()
        {
            GetNextItems(ref mFirstMailinglists, mMailinglists);
            OnPropertyChanged("FirstMailinglists");
            OnPropertyChanged("HasMoreMailinglists");
        }

        public ObservableCollection<EntityInfo> Items
        {
            get { return mItems; }
        }

        public bool HasItems
        {
            get
            {
                return mItems.Count > 0;
            }
        }

        public bool ShowSuggestions
        {
            get
            {
                return mTextModified && !string.IsNullOrEmpty(mText);
            }
        }

        public event EventHandler<SelectedSuggestionEventArgs> Selected;

        public void Select(EntityInfo item)
        {
            mRunningTask = EntityLookupAsync.AddLookups(item);
            if (Selected != null)
                Selected(this, new SelectedSuggestionEventArgs() { InfoSelected = item });
        }

        public void Select()
        {
            if (Selected != null)
                Selected(this, new SelectedSuggestionEventArgs() { TextSelected = mText });
        }

        private void GetFirstItems(ref EntityInfo[] result, ObservableCollection<EntityInfo> source)
        {
            if (result == null)
            {
                result = new EntityInfo[GetFirstItemsCount(source)];

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = source[i];
                }
            }
        }

        private void GetNextItems(ref EntityInfo[] result, ObservableCollection<EntityInfo> source)
        {
            if (result == null)
            {
                GetFirstItems(ref result, source);
                return;
            }

            int oldsize = result.Length;

            int newsize = Math.Min(result.Length + MAX_ITEMS, source.Count);

            if (newsize == oldsize)
                return;

            Array.Resize(ref result, newsize);

            for (int i = oldsize; i < result.Length; i++)
            {
                result[i] = source[i];
            }
        }

        private static int GetFirstItemsCount(ObservableCollection<EntityInfo> source)
        {
            return Math.Min(MAX_ITEMS, source.Count);
        }

        public bool CanGoOnline
        {
            get
            {
                return !mIsLoading && mText != null && mText.Length >= EntityLookupAsync.MAX_NAME_SEARCH_LENGTH;
            }
        }
    }

    public class SelectedSuggestionEventArgs : EventArgs
    {
        public string TextSelected { get; set; }
        public EntityInfo InfoSelected { get; set; }
    }
}
