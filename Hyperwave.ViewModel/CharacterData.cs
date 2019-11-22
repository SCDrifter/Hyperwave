using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.ViewModel
{
    public class CharacterData : EntityData
    {
        UIProperty<DateTime> mBirthday = new UIProperty<DateTime>("Birthday");
        UIProperty<string> mGender = new UIProperty<string>("Gender");
        UIProperty<string> mRace = new UIProperty<string>("Race");
        UIProperty<string> mBloodline = new UIProperty<string>("Bloodline");
        
        public DateTime Birthday
        {
            get
            {
                return mBirthday.DataValue;
            }
            set
            {
                if (mBirthday.DataValue != value)
                {
                    mBirthday.DataValue = value;
                    OnPropertyChanged("Birthday");
                }
            }
        }
        public string Gender
        {
            get
            {
                return mGender.DataValue;
            }
            set
            {
                if (mGender.DataValue != value)
                {
                    mGender.DataValue = value;
                    OnPropertyChanged("Gender");
                }
            }
        }
        public string Race
        {
            get
            {
                return mRace.DataValue;
            }
            set
            {
                if (mRace.DataValue != value)
                {
                    mRace.DataValue = value;
                    OnPropertyChanged("Race");
                }
            }
        }
        public string Bloodline
        {
            get
            {
                return mBloodline.DataValue;
            }
            set
            {
                if (mBloodline.DataValue != value)
                {
                    mBloodline.DataValue = value;
                    OnPropertyChanged("Bloodline");
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
                return "Employment History";
            }
        }

        public override string DescriptionTitle
        {
            get
            {
                return "Biography";
            }
        }
    }
}
