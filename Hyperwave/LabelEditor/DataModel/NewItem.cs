using Hyperwave.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hyperwave.LabelEditor.DataModel
{
    class NewItem : UIObject
    {
        private string mText = "";
        bool mIsValid = false;
        string mErrorText = null;
        bool mCanEdit = false;

        public string Text
        {
            get
            {
                return mText;
            }
            set
            {
                if (mText == value)
                    return;
                mText = value;
                OnPropertyChanged("Text");
                Validate();
            }
        }

        public bool CanEdit
        {
            get { return mCanEdit; }
            set
            {
                if (mCanEdit == value)
                    return;

                mCanEdit = true;
                OnPropertyChanged("CanEdit");
                if (!mCanEdit)
                    Text = "";
            }
        }

        static Regex mRegEx_LabelValidation = new Regex(@"^[_\p{N}\p{L} !@#$%^&*()_+-=\\/\][]+$");

        private void Validate()
        {
            if(mText.Length == 0)
            {
                IsValid = false;
                ErrorText = null;
                return;
            }

            if (mText.Length >= 40)
            {
                IsValid = false;
                ErrorText = "Labels must be less than is 40 characters";
                return;
            }

            if (!mRegEx_LabelValidation.IsMatch(mText))
            {
                IsValid = false;
                ErrorText = "Labels contains invalid characters";
                return;
            }

            IsValid = true;
            ErrorText = null;
        }

        public bool IsValid
        {
            get
            {
                return mIsValid;
            }
            set
            {
                if (value == mIsValid)
                    return;
                mIsValid = value;
                OnPropertyChanged("IsValid");
            }
        }

        public string ErrorText
        {
            get
            {
                return mErrorText;
            }
            set
            {
                if (mErrorText == value)
                    return;
                mErrorText = value;
                OnPropertyChanged("ErrorText");
                OnPropertyChanged("ShowError");
            }
        }

        public bool ShowError
        {
            get { return mErrorText != null; }
            set
            {
                if (value || mErrorText == null)
                    return;

                ErrorText = null;
            }
        }
    }
}
