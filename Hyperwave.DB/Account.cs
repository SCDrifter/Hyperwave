using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hyperwave.Auth;
using System.Data.SQLite;

namespace Hyperwave.DB
{
    public class Account : IComparable<Account>
    {
        AccountDatabase mDB;
        string mCharacterName;
        string mAccessToken;
        DateTime mExpires;
        string mRefreshToken;
        int mUnreadCount;
        long mLastMailId;
        bool mExpanded;

        internal Account(AccountDatabase db,long id,string name,AccessFlag permissions)
        {
            mDB = db;
            CharacterId = id;
            mCharacterName = name;
            Permissions = permissions;
        }

        internal Account(AccountDatabase db, SQLiteDataReader reader)
        {
            mDB = db;

            CharacterId = reader.GetInt64(0);
            mCharacterName = reader.GetString(1);
            mAccessToken = reader.GetString(2);
            mExpires = reader.GetDateTime(3);
            mRefreshToken = reader.GetString(4);
            mUnreadCount = reader.GetInt32(5);
            Permissions = (AccessFlag)reader.GetInt32(6);
            mLastMailId = reader.IsDBNull(7) ? -1: reader.GetInt64(7);
            mExpanded = reader.GetBoolean(8);
        }

        public long CharacterId { get; private set; }
        public string CharacterName
        {
            get
            {
                return mCharacterName;
            }
            set
            {
                if(mCharacterName != value)
                {
                    mCharacterName = value;
                    mDB.CharacterNameUpdated(this);
                }
            }
        }
        public string AccessToken
        {
            get
            {
                return mAccessToken;
            }
            set
            {
                if(mAccessToken != value)
                {
                    mAccessToken = value;
                    mDB.AccessTokenUpdated(this);
                }
            }
        }
        public DateTime Expires
        {
            get
            {
                return mExpires;
            }
            set
            {
                if(mExpires != value)
                {
                    mExpires = value;
                    mDB.ExpiresUpdated(this);
                }
            }
        }
        public string RefreshToken
        {
            get
            {
                return mRefreshToken;
            }
            set
            {
                if (mRefreshToken != value)
                {
                    mRefreshToken = value;
                    mDB.RefreshTokenUpdated(this);
                }
            }
        }
        public int UnreadCount
        {
            get
            {
                return mUnreadCount;
            }
            set
            {
                if(mUnreadCount != value)
                {
                    mUnreadCount = value;
                    mDB.UnreadCountUpdated(this);
                }
            }
        }

        public bool IsExpanded
        {
            get
            {
                return mExpanded;
            }
            set
            {
                if (mExpanded != value)
                {
                    mExpanded = value;
                    mDB.ExpandedUpdated(this);
                }
            }
        }


        public long LastMailId
        {
            get { return mLastMailId; }
            set
            {
                if (mLastMailId != value)
                {
                    mLastMailId = value;
                    mDB.LastMailIdUpdated(this);
                }
            }
        }

        public AccessFlag Permissions { get; private set; }

        public void UpdateAuthInfo(TokenInfo tokens)
        {
            AccessToken = tokens.AccessToken;
            RefreshToken = tokens.RefreshToken;
            Expires = tokens.Expires;
        }

        public void UpdateAuthInfo(TokenInfo tokens,AccessFlag permissions)
        {
            AccessToken = tokens.AccessToken;
            RefreshToken = tokens.RefreshToken;
            Expires = tokens.Expires;
            Permissions = permissions;
        }

        int IComparable<Account>.CompareTo(Account other)
        {
            return CharacterId.CompareTo(other.CharacterId);
        }
    }
}
