using Hyperwave.Config;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.DB
{
    public class AccountDatabase  : IDisposable
    {
        SQLiteConnection mDB = null;

        SQLiteCommand mCmd_UpdateAccount;
        SQLiteCommand mCmd_DeleteAccount;
        SQLiteCommand mCmd_AddAccount;

        SQLiteCommand mCmd_GetProperty;
        SQLiteCommand mCmd_SetProperty;


        SQLiteCommand mCmd_DeleteDraft;

        SortedSet<DraftMessage> mDrafts = new SortedSet<DraftMessage>();
        
        List<Account> mAccounts = new List<Account>();
        SortedSet<Account> mDirtyAccounts = new SortedSet<Account>();

        public AccountDatabase()
        {
            string fname = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            fname = Path.Combine(fname, "Hyperwave");

            if (!Directory.Exists(fname))
                Directory.CreateDirectory(fname);

            fname = Path.Combine(fname, "DB.sqlite");

            if (!File.Exists(fname))
                ExtractDB(fname);

            mDB = new SQLiteConnection(string.Format("DataSource={0};FailIfMissing=True;", fname));

            mCmd_AddAccount = new SQLiteCommand(@"INSERT INTO Accounts(CharacterId,CharacterName,Permissions) VALUES (@id,@name,@permissions)", mDB);
            mCmd_AddAccount.Parameters.AddWithValue("@id", 0L);
            mCmd_AddAccount.Parameters.AddWithValue("@name", "");
            mCmd_AddAccount.Parameters.AddWithValue("@permissions", 0);

            mCmd_UpdateAccount = new SQLiteCommand(
@"UPDATE 
    Accounts 
SET 
    CharacterName=@name,
    AccessToken=@access,
    RefreshToken=@refresh,
    Expires=@expires,
    UnreadCount=@unread,
    FolderExpanded=@expanded,
    LastMailId=@lastmailid
WHERE
    CharacterId = @id", mDB);

            mCmd_UpdateAccount.Parameters.AddWithValue("@id", 0L);
            mCmd_UpdateAccount.Parameters.AddWithValue("@name", "");
            mCmd_UpdateAccount.Parameters.AddWithValue("@access", "");
            mCmd_UpdateAccount.Parameters.AddWithValue("@refresh", "");
            mCmd_UpdateAccount.Parameters.AddWithValue("@expires", new DateTime());
            mCmd_UpdateAccount.Parameters.AddWithValue("@unread", 0);
            mCmd_UpdateAccount.Parameters.AddWithValue("@expanded", 0);
            mCmd_UpdateAccount.Parameters.AddWithValue("@lastmailid", 0);

            mCmd_DeleteAccount = new SQLiteCommand(@"DELETE FROM Accounts WHERE CharacterId = @id", mDB);
            mCmd_DeleteAccount.Parameters.AddWithValue("@id", 0L);
            
            mCmd_DeleteDraft = new SQLiteCommand(@"DELETE FROM Drafts WHERE DraftId=@draftid", mDB);
            mCmd_DeleteDraft.Parameters.AddWithValue("@draftid", null);

            mCmd_GetProperty = new SQLiteCommand(@"SELECT Value FROM DBProperties WHERE Property=@propname", mDB);
            mCmd_GetProperty.Parameters.AddWithValue("@propname", "");

            mCmd_SetProperty = new SQLiteCommand(@"INSERT OR REPLACE INTO DBProperties VALUES(@propname,@propvalue)", mDB);
            mCmd_SetProperty.Parameters.AddWithValue("@propname", "");
            mCmd_SetProperty.Parameters.AddWithValue("@propvalue", "");
        }

        private void ExtractDB(string fname)
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();

            using (var istream = asm.GetManifestResourceStream("Hyperwave.DB.DB.sqlite"))
            {
                using (var ostream = new FileStream(fname, FileMode.CreateNew, FileAccess.Write))
                {
                    istream.CopyTo(ostream);
                }
            }
        }

        public Account[] Accounts { get { return mAccounts.ToArray(); } }

        public void Load()
        {
            mDB.Open();

            using (SQLiteCommand load = new SQLiteCommand(@"SELECT CharacterId,CharacterName,AccessToken,Expires,RefreshToken,UnreadCount,Permissions,LastMailId,FolderExpanded FROM Accounts ORDER BY CharacterName", mDB))
            {
                using (SQLiteDataReader reader = (SQLiteDataReader)load.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        mAccounts.Add(new Account(this, reader));
                    }
                }
            }

            using (SQLiteCommand load = new SQLiteCommand(@"SELECT Drafts.DraftId,Drafts.CharacterId,Drafts.IsReply,Drafts.LastModified,Drafts.Subject,Drafts.Body,DraftRecipients.RecipientType,DraftRecipients.RecipientId FROM Drafts LEFT JOIN DraftRecipients ON(Drafts.DraftId = DraftRecipients.DraftId)", mDB))
            {
                using (SQLiteDataReader reader = load.ExecuteReader())
                {
                    DraftMessage msg = null;
                    while (reader.Read())
                    {
                        Guid id = reader.GetGuid(0);
                        if (msg == null || msg.DraftId != id)
                        {
                            msg = new DraftMessage(id) { DB = this };

                            mDrafts.Add(msg);

                            msg.AccountId = reader.GetInt64(1);
                            msg.IsReply = reader.GetBoolean(2);
                            msg.LastModified = reader.GetDateTime(3);
                            msg.Subject = reader.GetString(4);
                            msg.Body = reader.GetString(5);
                        }

                        if(!reader.IsDBNull(6) && !reader.IsDBNull(7))
                        {
                            msg.Recipients.Add(new UserCache.EntityInfo()
                            {
                                EntityType = (Common.EntityType)reader.GetInt32(6),
                                EntityID = reader.GetInt64(7)
                            });
                        }
                    }
                }
            }
        }

        public DraftMessage[] Drafts { get { return mDrafts.ToArray(); } }

        public async Task RemoveAccountAsync(Account account)
        {
            if (!mAccounts.Contains(account))
                return;

            using (var dbop = new DBOperation())
            {

                try
                {
                    mCmd_DeleteAccount.Parameters["@id"].Value = account.CharacterId;
                    await mCmd_DeleteAccount.ExecuteNonQueryAsync();
                    mAccounts.Remove(account);
                }
                catch (SQLiteException e)
                {
                    throw new AccountDatabaseException(string.Format("Unable to create account. Database error({0}).", e.ErrorCode));
                }
            }
        }

        public async Task<Account> AddAccountAsync(Auth.TokenInfo token,Auth.CharacterInfo charinfo,Auth.AccessFlag permissions)
        {
            using (var dbop = new DBOperation())
            {
                mCmd_AddAccount.Parameters["@id"].Value = charinfo.CharacterId;
                mCmd_AddAccount.Parameters["@name"].Value = charinfo.CharacterName;
                mCmd_AddAccount.Parameters["@permissions"].Value = (int)permissions;

                try
                {
                    await mCmd_AddAccount.ExecuteNonQueryAsync();
                    Account account = new Account(this, charinfo.CharacterId, charinfo.CharacterName, permissions);

                    mAccounts.Add(account);

                    account.UpdateAuthInfo(token);

                    await SaveChangesAsync();

                    return account;
                }
                catch (SQLiteException e)
                {
                    if (e.ErrorCode == 19)
                        throw new AccountExistsException();

                    throw new AccountDatabaseException(string.Format("Unable to create account. Database error({0}).", e.ErrorCode));
                } 
            }
        }

        public async Task SaveChangesAsync()
        {
            using (var dbop = new DBOperation())
            {
                try
                {
                    foreach (var i in mDirtyAccounts)
                    {
                        mCmd_UpdateAccount.Parameters["@id"].Value = i.CharacterId;
                        mCmd_UpdateAccount.Parameters["@name"].Value = i.CharacterName;
                        mCmd_UpdateAccount.Parameters["@access"].Value = i.AccessToken;
                        mCmd_UpdateAccount.Parameters["@refresh"].Value = i.RefreshToken;
                        mCmd_UpdateAccount.Parameters["@expires"].Value = i.Expires;
                        mCmd_UpdateAccount.Parameters["@unread"].Value = i.UnreadCount;
                        mCmd_UpdateAccount.Parameters["@expanded"].Value = i.IsExpanded;
                        mCmd_UpdateAccount.Parameters["@lastmailid"].Value = i.LastMailId;

                        await mCmd_UpdateAccount.ExecuteNonQueryAsync();
                    }
                }
                catch (SQLiteException e)
                {
                    throw new AccountDatabaseException(string.Format("Unable to save account changes. Database error({0}).", e.ErrorCode));
                } 
            }
        }

        public void SaveChanges()
        {
            using (var dbop = new DBOperation())
            {
                try
                {
                    foreach (var i in mDirtyAccounts)
                    {
                        mCmd_UpdateAccount.Parameters["@id"].Value = i.CharacterId;
                        mCmd_UpdateAccount.Parameters["@name"].Value = i.CharacterName;
                        mCmd_UpdateAccount.Parameters["@access"].Value = i.AccessToken;
                        mCmd_UpdateAccount.Parameters["@refresh"].Value = i.RefreshToken;
                        mCmd_UpdateAccount.Parameters["@expires"].Value = i.Expires;
                        mCmd_UpdateAccount.Parameters["@unread"].Value = i.UnreadCount;
                        mCmd_UpdateAccount.Parameters["@expanded"].Value = i.IsExpanded;
                        mCmd_UpdateAccount.Parameters["@lastmailid"].Value = i.LastMailId;

                        mCmd_UpdateAccount.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException e)
                {
                    throw new AccountDatabaseException(string.Format("Unable to save account changes. Database error({0}).", e.ErrorCode));
                }
            }
        }

        public async Task SaveDraftAsync(DraftMessage msg)
        {
            int count = 0;
            using (var trns = mDB.BeginTransaction())
            {
                using (var dbop = new DBOperation())
                {
                    try
                    {
                        var Cmd_SaveDraft = new SQLiteCommand(@"INSERT OR REPLACE INTO Drafts (DraftId,CharacterId,LastModified,Subject,IsReply,Body) VALUES(@draftid,@charid,@lastmodified,@subject,@isreply,@body)", mDB);
                        Cmd_SaveDraft.Transaction = trns;
                        Cmd_SaveDraft.Parameters.AddWithValue("@draftid", msg.DraftId.ToString());
                        Cmd_SaveDraft.Parameters.AddWithValue("@lastmodified", msg.LastModified = DateTime.Now);
                        Cmd_SaveDraft.Parameters.AddWithValue("@charid", msg.AccountId);
                        Cmd_SaveDraft.Parameters.AddWithValue("@subject", msg.Subject);
                        Cmd_SaveDraft.Parameters.AddWithValue("@isreply", msg.IsReply);
                        Cmd_SaveDraft.Parameters.AddWithValue("@body", msg.Body);
                        count = await Cmd_SaveDraft.ExecuteNonQueryAsync();

                        var Cmd_DeleteDraftReceipients = new SQLiteCommand(@"DELETE FROM DraftRecipients WHERE DraftId=@draftid", mDB);
                        Cmd_DeleteDraftReceipients.Transaction = trns;
                        Cmd_DeleteDraftReceipients.Parameters.AddWithValue("@draftid", msg.DraftId.ToString());

                        count = await Cmd_DeleteDraftReceipients.ExecuteNonQueryAsync();

                        var Cmd_AddDraftReceipient = new SQLiteCommand(@"INSERT OR REPLACE INTO DraftRecipients (DraftId,RecipientType,RecipientId) VALUES(@draftid,@type,@id)", mDB);
                        Cmd_AddDraftReceipient.Transaction = trns;
                        Cmd_AddDraftReceipient.Parameters.AddWithValue("@draftid", null);
                        Cmd_AddDraftReceipient.Parameters.AddWithValue("@type", null);
                        Cmd_AddDraftReceipient.Parameters.AddWithValue("@id", null);

                        foreach (var i in msg.Recipients)
                        {
                            Cmd_AddDraftReceipient.Parameters["@draftid"].Value = msg.DraftId.ToString();
                            Cmd_AddDraftReceipient.Parameters["@type"].Value = (int)i.EntityType;
                            Cmd_AddDraftReceipient.Parameters["@id"].Value = i.EntityID;

                            count = await Cmd_AddDraftReceipient.ExecuteNonQueryAsync();
                        }

                        trns.Commit();
                    }
                    catch (SQLiteException e)
                    {
                        trns.Rollback();
                        throw new AccountDatabaseException(string.Format("Unable to save draft. Database error({0}).", e.ErrorCode));
                    }
                }
            }
        }

        public async Task DeleteDraftAsync(DraftMessage msg)
        {
            using (var dbop = new DBOperation())
            {
                try
                {
                    mCmd_DeleteDraft.Parameters["@draftid"].Value = msg.DraftId.ToString();
                    await mCmd_DeleteDraft.ExecuteNonQueryAsync();
                    mDrafts.Remove(msg);
                }
                catch (SQLiteException e)
                {
                    throw new AccountDatabaseException(string.Format("Unable to save draft. Database error({0}).", e.ErrorCode));
                }
            }

        }

        public string GetProperty(string name, string defaultvalue)
        {
            using (var dbop = new DBOperation())
            {
                mCmd_GetProperty.Parameters["@propname"].Value = name;
                return (mCmd_GetProperty.ExecuteScalar() ?? defaultvalue).ToString();
            }
        }

        public void SetProperty(string name, string value)
        {
            using (var dbop = new DBOperation())
            {
                mCmd_SetProperty.Parameters["@propname"].Value = name;
                mCmd_SetProperty.Parameters["@propvalue"].Value = value;
                mCmd_SetProperty.ExecuteNonQuery();
            }
        }

        public async Task<string> GetPropertyAsync(string name, string defaultvalue)
        {
            using (var dbop = new DBOperation())
            {
                mCmd_GetProperty.Parameters["@propname"].Value = name;
                return (await mCmd_GetProperty.ExecuteScalarAsync() ?? defaultvalue).ToString();
            }
        }

        public async Task SetPropertyAsync(string name, string value)
        {
            using (var dbop = new DBOperation())
            {
                mCmd_SetProperty.Parameters["@propname"].Value = name;
                mCmd_SetProperty.Parameters["@propvalue"].Value = value;
                await mCmd_SetProperty.ExecuteNonQueryAsync();
            }
        }

        internal void CharacterNameUpdated(Account account)
        {
            mDirtyAccounts.Add(account);
        }

        internal void AccessTokenUpdated(Account account)
        {
            mDirtyAccounts.Add(account);
        }

        internal void RefreshTokenUpdated(Account account)
        {
            mDirtyAccounts.Add(account);
        }

        internal void ExpiresUpdated(Account account)
        {
            mDirtyAccounts.Add(account);
        }

        internal void UnreadCountUpdated(Account account)
        {
            mDirtyAccounts.Add(account);
        }

        internal void ExpandedUpdated(Account account)
        {
            mDirtyAccounts.Add(account);
        }

        internal void LastMailIdUpdated(Account account)
        {
            mDirtyAccounts.Add(account);
        }

        public void Dispose()
        {
            if (mDB == null)
            {
                mCmd_AddAccount.Dispose();
                mCmd_DeleteAccount.Dispose();
                mCmd_UpdateAccount.Dispose();
                mCmd_DeleteDraft.Dispose();
                mCmd_GetProperty.Dispose();
                mCmd_SetProperty.Dispose();
                mDB.Dispose();
                mDB = null;
                mCmd_AddAccount = null;
                mCmd_DeleteAccount = null;
                mCmd_UpdateAccount = null;
                mCmd_DeleteDraft = null;
                mCmd_GetProperty = null;
                mCmd_SetProperty = null;
            }
        }
    }

    [Serializable]
    internal class AccountDatabaseException : Exception
    {
        public AccountDatabaseException()
        {
        }

        public AccountDatabaseException(string message) : base(message)
        {
        }

        public AccountDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccountDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class AccountExistsException : AccountDatabaseException
    {
        public AccountExistsException()
            :base("An account with that name already exists")
        {
        }

        public AccountExistsException(string message) : base(message)
        {
        }

        public AccountExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccountExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
