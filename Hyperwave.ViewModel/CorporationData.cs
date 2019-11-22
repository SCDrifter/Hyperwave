using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class CorporationData : EntityData
    {
        UIProperty<string> mTicker = new UIProperty<string>("Ticker");
        UIProperty<DateTime> mFounded = new UIProperty<DateTime>("Date Founded");
        UIProperty<double> mTaxRate = new UIProperty<double>("Tax Rate");
        UIProperty<MailRecipient> mCreator = new UIProperty<MailRecipient>("Creator");
        WebLinkProperty mUrl = new WebLinkProperty("Website","");
        UIProperty<MailRecipient> mCEO = new UIProperty<MailRecipient>("CEO");

        public string Ticker
        {
            get
            {
                return mTicker.DataValue;
            }
            set
            {
                if (mTicker.DataValue != value)
                {
                    mTicker.DataValue = value;
                    OnPropertyChanged("Ticker");
                }
            }
        }
        public DateTime Founded
        {
            get
            {
                return mFounded.DataValue;
            }
            set
            {
                if (mFounded.DataValue != value)
                {
                    mFounded.DataValue = value;
                    OnPropertyChanged("Founded");
                }
            }
        }
        public double TaxRate
        {
            get
            {
                return mTaxRate.DataValue;
            }
            set
            {
                if (mTaxRate.DataValue != value)
                {
                    mTaxRate.DataValue = value;
                    OnPropertyChanged("TaxRate");
                }
            }
        }

        public MailRecipient Creator
        {
            get
            {
                return mCreator.DataValue;
            }
            set
            {
                if (mCreator.DataValue != value)
                {
                    mCreator.DataValue = value;
                    OnPropertyChanged("Creator");
                }
            }
        }

        public MailRecipient CEO
        {
            get
            {
                return mCEO.DataValue;
            }
            set
            {
                if (mCEO.DataValue != value)
                {
                    mCEO.DataValue = value;
                    OnPropertyChanged("CEO");
                }
            }
        }
        public string Url
        {
            get
            {
                return mUrl.DataValue;
            }
            set
            {
                if (mUrl.DataValue != value)
                {
                    mUrl.DataValue = value;
                    mUrl.LinkText = value;
                    OnPropertyChanged("Url");
                }
            }
        }

        public override bool HasHistory
        {
            get
            {
                return true;
            }
        }

        public override string HistoryLabel
        {
            get
            {
                return "Alliance History";
            }
        }

        public override string DescriptionTitle
        {
            get
            {
                return "Description";
            }
        }
    }
}
