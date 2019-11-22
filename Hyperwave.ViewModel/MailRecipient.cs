using Hyperwave.Common;

namespace Hyperwave.ViewModel
{
    public class MailRecipient : UIObject
    {
        string mName;
        long mId;
        EntityType mType;
        string mImageUrl16;
        string mImageUrl32;
        string mImageUrl64;
        string mImageUrl128;

        public MailRecipient()
        {

        }

        public MailRecipient(EntityType type,long id)
        {
            mId = id;
            mType = type;
            //if (id == -1)
                mName = string.Format("(Unknown {0})", type);
            //else
            //    mName = string.Format("{0}{1} #{2}", type.Substring(0, 1).ToUpper(), type.Substring(1), Id);
        }
        public virtual long Id
        {
            get
            {
                return mId;
            }

            set
            {
                if (mId == value)
                    return;

                mId = value;
                OnPropertyChanged("Id");
            }
        }

        public EntityType Type
        {
            get
            {
                return mType;
            }

            set
            {
                if (mType == value)
                    return;

                mType = value;
                OnPropertyChanged("Type");
            }
        }

        public string Name
        {
            get
            {
                return mName;
            }

            set
            {
                if (mName == value)
                    return;
                
                mName = value;
                OnPropertyChanged("Name");
            }
        }


        public string ImageUrl16
        {
            get
            {
                return mImageUrl16;
            }

            set
            {
                if (mImageUrl16 != value)
                {
                    mImageUrl16 = value;
                    OnPropertyChanged("ImageUrl16");
                }
            }
        }

        public string ImageUrl32
        {
            get
            {
                return mImageUrl32;
            }

            set
            {
                if (mImageUrl32 != value)
                {
                    mImageUrl32 = value;
                    OnPropertyChanged("ImageUrl32");
                }
            }
        }

        public string ImageUrl64
        {
            get
            {
                return mImageUrl64;
            }

            set
            {
                if (mImageUrl64 != value)
                {
                    mImageUrl64 = value;
                    OnPropertyChanged("ImageUrl64");
                }
            }
        }

        public string ImageUrl128
        {
            get
            {
                return mImageUrl128;
            }

            set
            {
                if (mImageUrl128 != value)
                {
                    mImageUrl128 = value;
                    OnPropertyChanged("ImageUrl128");
                }
            }
        }
    }
}