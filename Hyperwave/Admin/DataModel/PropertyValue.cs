using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hyperwave.Admin.DataModel
{
    public class PropertyValue
    {
        DataTemplate mValueTemplate = GetDefaultValueTemplate();
        
        public string Title { get; set; }

        public object Value
        {
            get;
            set;
        }
        public DataTemplate ValueTemplate
        {
            get { return mValueTemplate; }
            set { mValueTemplate = value; }
        }

        
        private static DataTemplate GetDefaultValueTemplate()
        {
            var app = Application.Current;

            var ret = (DataTemplate)app.Resources["PropertyValue_DefaultDataTemplate"];

            if (ret == null)
                throw new Exception("Valid resource not found");

            return ret;
        }
    }
}
