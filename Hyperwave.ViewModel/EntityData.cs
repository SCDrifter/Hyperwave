using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Hyperwave.ViewModel
{
    public abstract class EntityData : UIObject
    {
        MailRecipient mSubject;
        MailRecipient mPrimaryMembership;
        MailRecipient mSecondaryMembership;
        bool mHasSecurityStatus;
        double mSecurityStatus;
        bool mInfoLoaded;
        string mDescription;
        bool mHistoryLoaded;
        bool mLoadFailed;
        HistoryItem[] mHistory;

        UIProperty[] mProperties;

        public EntityData()
        {
            mProperties = UIProperty.GetProperties(this);
        }

        public MailRecipient Subject
        {
            get
            {
                return mSubject;
            }
            set
            {
                if (mSubject != value)
                {
                    mSubject = value;
                    OnPropertyChanged("Subject");
                }
            }
        }
        public bool HasPrimaryMembership
        {
            get
            {
                return mPrimaryMembership != null;
            }
        }
        public MailRecipient PrimaryMembership
        {
            get
            {
                return mPrimaryMembership;
            }
            set
            {
                if (mPrimaryMembership != value)
                {
                    mPrimaryMembership = value;
                    OnPropertyChanged("PrimaryMembership");
                    OnPropertyChanged("HasPrimaryMembership");
                }
            }
        }
        public bool HasSecondaryMembership
        {
            get
            {
                return mSecondaryMembership != null;
            }
        }
        public MailRecipient SecondaryMembership
        {
            get
            {
                return mSecondaryMembership;
            }
            set
            {
                if (mSecondaryMembership != value)
                {
                    mSecondaryMembership = value;
                    OnPropertyChanged("SecondaryMembership");
                    OnPropertyChanged("HasSecondaryMembership");
                }
            }
        }
        public bool HasSecurityStatus
        {
            get
            {
                return mHasSecurityStatus;
            }
            set
            {
                if (mHasSecurityStatus != value)
                {
                    mHasSecurityStatus = value;
                    OnPropertyChanged("HasSecurityStatus");
                }
            }
        }
        public double SecurityStatus
        {
            get
            {
                return mSecurityStatus;
            }
            set
            {
                if (mSecurityStatus != value)
                {
                    mSecurityStatus = value;
                    OnPropertyChanged("SecurityStatus");
                }
            }
        }
        public bool HasAnyInfo
        {
            get
            {
                return !mLoadFailed && (mInfoLoaded || mHistoryLoaded);
            }
        }

        public bool IsLoading
        {
            get
            {
                return !mLoadFailed && !mInfoLoaded && !mHistoryLoaded;
            }
        }

        public bool InfoLoaded
        {
            get
            {
                return mInfoLoaded;
            }
            set
            {
                if (mInfoLoaded != value)
                {
                    mInfoLoaded = value;
                    OnPropertyChanged("InfoLoaded");
                    OnPropertyChanged("HasAnyInfo");
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        public bool HistoryLoaded
        {
            get
            {
                return mHistoryLoaded;
            }
            set
            {
                if (mHistoryLoaded != value)
                {
                    mHistoryLoaded = value;
                    OnPropertyChanged("HistoryLoaded");
                    OnPropertyChanged("HasAnyInfo");
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        public bool LoadFailed
        {
            get
            {
                return mLoadFailed;
            }
            set
            {
                if (mLoadFailed != value)
                {
                    mLoadFailed = value;
                    OnPropertyChanged("LoadFailed");
                    OnPropertyChanged("IsLoading");
                    OnPropertyChanged("HasAnyInfo");
                }
            }
        }

        public bool HasDescription
        {
            get
            {
                return Description != null;
            }
        }
        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                if (mDescription != value)
                {
                    mDescription = value;
                    OnPropertyChanged("Description");
                    OnPropertyChanged("HasDescription");
                }
            }
        }
        

        public abstract string DescriptionTitle
        {
            get;
        }
        
        public HistoryItem[] History
        {
            get
            {
                return mHistory;
            }
            set
            {
                if (mHistory != value)
                {
                    mHistory = value;
                    OnPropertyChanged("History");
                    OnPropertyChanged("HasHistory");
                }
            }
        }

        public abstract bool HasHistory
        {
            get;
        }

        public abstract string HistoryLabel
        {
            get;
        }

        public UIProperty[] Properties { get { return mProperties; } }
    }

    public class HistoryItem : UIObject
    {
        MailRecipient mOrganization;
        bool mClosed;
        DateTime mStartDate;
        bool mHasEndDate;
        bool mHasStartDate;
        DateTime mEndDate;
        private string mEndDateText;
        private string mStartDateText;

        public MailRecipient Organization
        {
            get
            {
                return mOrganization;
            }
            set
            {
                if (mOrganization != value)
                {
                    if(mOrganization != null)
                        mOrganization.UnregisterHandler("Name", mOrganization_NameChanged);

                    mOrganization = value;

                    if (mOrganization != null)
                        mOrganization.RegisterHandler("Name",mOrganization_NameChanged);

                    OnPropertyChanged("Organization");
                    OnPropertyChanged("OrganizationNameAndStatus");
                }
            }
        }

        private void mOrganization_NameChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("OrganizationNameAndStatus");
        }

        public bool Closed
        {
            get
            {
                return mClosed;
            }
            set
            {
                if (mClosed != value)
                {
                    mClosed = value;
                    OnPropertyChanged("Closed");
                    OnPropertyChanged("OrganizationNameAndStatus");
                }
            }
        }

        public string OrganizationNameAndStatus
        {
            get
            {
                return string.Format("{0}{1}", Organization.Name, Closed ? "(Closed)" : "");
            }
        }

        public DateTime StartDate
        {
            get
            {
                return mStartDate;
            }
            set
            {
                if (mStartDate != value)
                {
                    mStartDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }

        public string StartDateText
        {
            get
            {
                return mStartDateText;
            }
            set
            {
                if (mStartDateText != value)
                {
                    mStartDateText = value;
                    OnPropertyChanged("StartDateText");
                }
            }
        }
        public bool HasEndDate
        {
            get
            {
                return mHasEndDate;
            }
            set
            {
                if (mHasEndDate != value)
                {
                    mHasEndDate = value;
                    OnPropertyChanged("HasEndDate");
                }
            }
        }

        public bool HasStartDate
        {
            get
            {
                return mHasStartDate;
            }
            set
            {
                if (mHasStartDate != value)
                {
                    mHasStartDate = value;
                    OnPropertyChanged("HasStartDate");
                }
            }
        }
        public DateTime EndDate
        {
            get
            {
                return mEndDate;
            }
            set
            {
                if (mEndDate != value)
                {
                    mEndDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        public string EndDateText
        {
            get
            {
                return mEndDateText;
            }
            set
            {
                if (mEndDateText != value)
                {
                    mEndDateText = value;
                    OnPropertyChanged("EndDateText");
                }
            }
        }
    }
}
