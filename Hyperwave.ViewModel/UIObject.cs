using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hyperwave.ViewModel
{
    public class UIObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        SortedSet<string> mDeferredChanges = null;
        int mDeferredChangeRefCount = 0;

        public void DeferPropertyChanges()
        {
            if (mDeferredChangeRefCount++ > 0)
                return;
            mDeferredChanges = new SortedSet<string>();
        }

        public void ApplyDeferredPropertyChanges()
        {
            if (mDeferredChangeRefCount == 0)
                return;
            if (--mDeferredChangeRefCount > 0)
                return;

            var items = mDeferredChanges;
            mDeferredChanges = null;

            foreach(string i in items)
            {
                OnPropertyChanged(i);
            }

            items.Clear();
        }

        protected virtual void OnPropertyChanged(string property_name)
        {
            if(mDeferredChanges != null)
            {
                mDeferredChanges.Add(property_name);
                return;
            }
            var e = new PropertyChangedEventArgs(property_name);

            if (PropertyChanged != null)
                PropertyChanged(this, e);

            EventHandler<PropertyChangedEventArgs> prophandler;

            if (mPropHandlers.TryGetValue(property_name, out prophandler))
                prophandler(this, e);
        }

        Dictionary<string, EventHandler<PropertyChangedEventArgs>> mPropHandlers = new Dictionary<string, EventHandler<PropertyChangedEventArgs>>();

        public void RegisterHandler(string property, EventHandler<PropertyChangedEventArgs> handler)
        {
            EventHandler<PropertyChangedEventArgs> current;
            if (!mPropHandlers.TryGetValue(property, out current))
                mPropHandlers.Add(property, handler);
            else
            {
                current += handler;
                mPropHandlers[property] = current;
            }
        }

        public void UnregisterHandler(string property, EventHandler<PropertyChangedEventArgs> handler)
        {
            EventHandler<PropertyChangedEventArgs> current;
            if (!mPropHandlers.TryGetValue(property, out current))
                return;

            current -= handler;

            if (current == null)
                mPropHandlers.Remove(property);
            else
                mPropHandlers[property] = current;
        }
    }
    public class DeferUIObjectChanges : IDisposable
    {
        UIObject mSource;

        public DeferUIObjectChanges(UIObject source)
        {
            mSource = source;
            mSource.DeferPropertyChanges();
        }

        public void Dispose()
        {
            if (mSource == null)
                return;

            mSource.ApplyDeferredPropertyChanges();
            mSource = null;
        }
    }
}