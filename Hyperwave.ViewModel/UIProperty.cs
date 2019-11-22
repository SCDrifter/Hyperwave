using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Hyperwave.ViewModel
{
    public abstract class UIProperty : UIObject
    {
        string mLabel;

        protected UIProperty(string label)
        {
            mLabel = label;
        }

        public string Label { get { return mLabel; } }

        public abstract object Value { get; set; }

        public static UIProperty[] GetProperties(object source)
        {
            List<UIProperty> props = new List<UIProperty>();
                        
            foreach (var i in source.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
               UIProperty prop = i.GetValue(source) as UIProperty;
                if (prop != null)
                    props.Add(prop);
            }

            return props.ToArray();
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", mLabel, Value);
        }
    }

    public class UIProperty<T> : UIProperty
    {
        T mValue;
        public UIProperty(string label, T initial_value = default(T))
            : base(label)
        {
            mValue = initial_value;
        }

        

        public T DataValue
        {
            get
            {
                return mValue;
            }
            set
            {
                if(!Equals(mValue,value))
                {
                    mValue = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public override object Value
        {
            get
            {
                return DataValue;
            }
            set
            {
                DataValue = (T)value;
            }
        }
    }
    

    public class  WebLinkProperty: UIProperty<string>
    {
        string mLinkText;
        public WebLinkProperty(string label, string link_text, string initial_value = default(string))
            : base(label, initial_value)
        {
            mLinkText = link_text;
        }

        public string LinkText
        {
            get
            {
                return mLinkText;
            }
            set
            {
                if (mLinkText != value)
                {
                    mLinkText = value;
                    OnPropertyChanged("LinkText");
                }
            }
        }
    }
    
}
