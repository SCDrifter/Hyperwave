using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class AllianceData : EntityData
    {
        UIProperty<string> mTicker = new UIProperty<string>("Ticker");
        UIProperty<DateTime> mFounded = new UIProperty<DateTime>("Date Founded");
        UIProperty<MailRecipient> mCreatorCharacter = new UIProperty<MailRecipient>("Created By");
        UIProperty<MailRecipient> mCreatorCorp = new UIProperty<MailRecipient>("Created By Corp");
        UIProperty<MailRecipient> mExecutor = new UIProperty<MailRecipient>("Exxector Corp");

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
        public MailRecipient CreatorCharacter
        {
            get
            {
                return mCreatorCharacter.DataValue;
            }
            set
            {
                if (mCreatorCharacter.DataValue != value)
                {
                    mCreatorCharacter.DataValue = value;
                    OnPropertyChanged("CreatorCharacter");
                }
            }
        }
        public MailRecipient CreatorCorp
        {
            get
            {
                return mCreatorCorp.DataValue;
            }
            set
            {
                if (mCreatorCorp.DataValue != value)
                {
                    mCreatorCorp.DataValue = value;
                    OnPropertyChanged("CreatorCorp");
                }
            }
        }

        public MailRecipient Executor
        {
            get
            {
                return mExecutor.DataValue;
            }
            set
            {
                if (mExecutor.DataValue != value)
                {
                    mExecutor.DataValue = value;
                    OnPropertyChanged("Executor");
                }
            }
        }

        public override bool HasHistory
        {
            get
            {
                return false;
            }
        }

        public override string HistoryLabel
        {
            get
            {
                return null;
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
