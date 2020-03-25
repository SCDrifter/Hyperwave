using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eve.Api.Api;
using Eve.Api.Model;
using Hyperwave.Config;
using System.Data.SQLite;
using Hyperwave.Common;
using System.Threading;
using System.IO;

namespace Hyperwave.UserCache
{
    public static class EntityLookup
    {
        public const int MAX_NAME_LOOKUP = 255;

        public const int MAX_NAME_SEARCH_LENGTH = 5;

        public static void LookupIDs(params EntityInfo[] entities)
        {
            Common.InitializeDB();
            LookupIDsInternal(entities, true);
        }

        public static void AddLookups(params EntityInfo[] items)
        {
            Common.InitializeDB();
            AddLookupsInternal(items);
        }

        public static EntityInfo[] Search(string text, SearchOptions options,EntityType? type = null)
        {
            Common.InitializeDB();

            List<EntityInfo> ret = new List<EntityInfo>();


            if (options.HasFlag(SearchOptions.Offline))
                SearchOffline(ret, text, options.HasFlag(SearchOptions.ExactMatch), type);
            if (options.HasFlag(SearchOptions.Online))
                SearchOnline(ret, text, options.HasFlag(SearchOptions.ExactMatch), type);

            return ret.ToArray();

        }


        public static int PurgeRecords(EntityType type)
        {
            Common.InitializeDB();
            using (SQLiteCommand cmd = new SQLiteCommand(Common.DB))
            {
                cmd.CommandText = @"DELETE FROM Names WHERE EntityType=@type";
                cmd.Parameters.AddWithValue("@type", (int)type);
                return cmd.ExecuteNonQuery();
            }
        }

        

        private static void LookupIDsInternal(EntityInfo[] entities, bool cacheinfo)
        {
            List<EntityInfo> workinglist = new List<EntityInfo>(entities);
            List<EntityInfo> createdlist = new List<EntityInfo>();

            LookupPredefinedIDs(workinglist);

            LookupIDsInDatabase(workinglist);

            if (workinglist.Count > 0)
                LookupByIDAndType(workinglist, createdlist);

            foreach (var i in workinglist)
            {
                i.OnFinished(false);
            }
            if (cacheinfo)
                AddLookupsInternal(createdlist);
        }

        private static void LookupPredefinedIDs(List<EntityInfo> workinglist)
        {
            for (int i = 0; i < workinglist.Count;)
            {
                var item = workinglist[i];
                switch (item.EntityID)
                {
                    case 0:
                        item.Name = string.Format("(No {0})", item.EntityType);
                        workinglist.RemoveAt(i);
                        item.OnFinished(true);
                        continue;
                    case -1:
                        item.Name = string.Format("(Unknown {0})", item.EntityType);
                        workinglist.RemoveAt(i);
                        item.OnFinished(true);
                        continue;
                }
                i++;
            }
        }


        private static void AddLookupsInternal(IEnumerable<EntityInfo> items)
        {
            using (var trns = Common.DB.BeginTransaction())
            {
                using (var cmd = new SQLiteCommand(@"INSERT OR REPLACE INTO Names(EntityType,EntityID,Name) VALUES(@type,@id,@name)", Common.DB, trns))
                {
                    foreach (var i in items)
                    {
                        cmd.Parameters.AddWithValue("@type", (int)i.EntityType);
                        cmd.Parameters.AddWithValue("@id", i.EntityID);
                        cmd.Parameters.AddWithValue("@name", i.Name);

                        try
                        {
                            var result = cmd.ExecuteNonQuery();
                        }
                        catch (SQLiteException e)
                        {
                            throw e;
                        }
                    }

                }
                trns.Commit();
            }
        }

        private static void LookupIDsInDatabase(List<EntityInfo> entities)
        {
            string items = string.Join(",", from i in entities select i.EntityID);
            using (SQLiteCommand cmd = new SQLiteCommand(Common.DB))
            {
                cmd.CommandText = "SELECT EntityID,EntityType,Name FROM Names WHERE EntityID = @id";
                cmd.Parameters.AddWithValue("@id", 0L);
                for (int i = 0; i < entities.Count; i++)
                {
                    var item = entities[i];
                    cmd.Parameters["@id"].Value = item.EntityID;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long id = reader.GetInt64(0);
                            byte type = reader.GetByte(1);
                            string name = reader.GetString(2);

                            item.Name = name;
                            item.EntityType = (EntityType)type;
                            entities.RemoveAt(i);
                            i--;
                            item.OnFinished(true);
                        }
                    }
                }
            }
        }

        private static void LookupByIDAndType(List<EntityInfo> workinglist, List<EntityInfo> createdlist)
        {
            RemoveMailingListIds(workinglist);
            if (workinglist.Count == 0)
                return;

            UniverseApi api = new UniverseApi();
            Queue<List<int?>> queue = new Queue<List<int?>>();

            queue.Enqueue(new List<int?>(from x in workinglist where x.EntityType != EntityType.Mailinglist select (int?)x.EntityID));

            while (queue.Count > 0)
            {
                var idlist = queue.Dequeue();
                if (PerformLookup(workinglist, idlist, api, createdlist))
                    continue;
                if (idlist.Count == 1)
                {
                    RegisterMailingList(idlist[0].Value);
                    continue;
                }
                int halfpoint = idlist.Count / 2;
                if (halfpoint > 0)
                    queue.Enqueue(idlist.GetRange(0, halfpoint));
                queue.Enqueue(idlist.GetRange(halfpoint, idlist.Count - halfpoint));

            }
        }

        private static bool PerformLookup(List<EntityInfo> workinglist, List<int?> idlist, UniverseApi api, List<EntityInfo> createdlist)
        {

            List<PostUniverseNames200Ok> names;
            try
            {
                names = api.PostUniverseNames(idlist);
            }
            catch (Eve.Api.Client.ApiException e)
            {
                if (e.ErrorCode == 404)
                    return false;
                throw;
            }


            foreach (var i in names)
            {
                var info = workinglist.Find((j) => i.Id.Value == j.EntityID);

                EntityType type = EntityType.Alliance;

                switch (i.Category ?? PostUniverseNames200Ok.CategoryEnum.Station)
                {
                    case PostUniverseNames200Ok.CategoryEnum.Alliance:
                        type = EntityType.Alliance;
                        break;
                    case PostUniverseNames200Ok.CategoryEnum.Corporation:
                        type = EntityType.Corporation;
                        break;
                    case PostUniverseNames200Ok.CategoryEnum.Character:
                        type = EntityType.Character;
                        break;
                    default:
                        continue;
                }
                info.Name = i.Name;
                info.EntityType = type;
                workinglist.Remove(info);
                info.OnFinished(true);
                createdlist.Add(info);
            }

            return true;
        }

        private static void RemoveMailingListIds(List<EntityInfo> entities)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(Common.DB))
            {
                cmd.CommandText = "SELECT * FROM MailingListIds";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(0);
                        var info = entities.Find((j) => id == j.EntityID);

                        if (info == null)
                            continue;

                        info.EntityType = EntityType.Mailinglist;
                        entities.Remove(info);
                    }
                }
            }
        }
        private static void RegisterMailingList(long id)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(Common.DB))
            {
                cmd.CommandText = "INSERT OR IGNORE INTO MailingListIds VALUES (@id)";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SearchOffline(List<EntityInfo> ret, string text, bool exactmatch, EntityType? restrict_type)
        {
            List<string> names = new List<string>();

            if (!exactmatch)
            {
                using (var cmd_search = new SQLiteCommand("SELECT * FROM NameSearch WHERE Name MATCH @search || '*'", Common.DB))
                {
                    cmd_search.Parameters.AddWithValue("@search", text);
                    using (var reader = cmd_search.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add(reader.GetString(0));
                        }
                    }
                }
                if (names.Count == 0)
                    return;
            }

            SQLiteCommand cmd;
            if (exactmatch)
            {
                cmd = new SQLiteCommand("SELECT * FROM Names WHERE Name = @search COLLATE NOCASE", Common.DB);
                cmd.Parameters.AddWithValue("@search", text);
            }
            else
            {
                cmd = new SQLiteCommand("SELECT * FROM Names WHERE Name IN ({Search})", Common.DB);
                cmd.AddArrayParameters(names, "Search");
            }


            if (restrict_type.HasValue)
            {
                cmd.CommandText += " and EntityType = @type";
                cmd.Parameters.AddWithValue("@type", (int)restrict_type.Value);
            }

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    long id = reader.GetInt64(0);
                    byte type = reader.GetByte(1);
                    string name = reader.GetString(2);

                    EntityInfo item = new EntityInfo();
                    item.EntityID = id;
                    item.Name = name;
                    item.EntityType = (EntityType)type;

                    ret.Add(item);
                }
            }
        }
        private static void SearchOnline(List<EntityInfo> ret, string text, bool exactmatch, EntityType? restrict_type)
        {
            SortedSet<long> hasitems = new SortedSet<long>(from id in ret select id.EntityID);
            List<EntityInfo> newitems = new List<EntityInfo>();
            if (text.Length <= MAX_NAME_SEARCH_LENGTH)
                return;
            try
            {
                List<string> catlist;
                switch (restrict_type)
                {
                    case EntityType.Character:
                        catlist = new List<string>() { "character" };
                        break;
                    case EntityType.Corporation:
                        catlist = new List<string>() { "corporation" };
                        break;
                    case EntityType.Alliance:
                        catlist = new List<string>() { "alliance" };
                        break;
                    case EntityType.Mailinglist:
                        return;
                    default:
                        catlist = new List<string>()
                        {
                            "character","corporation","alliance"
                        };
                        break;
                }

                SearchApi api = new SearchApi();
                var list = api.GetSearch(categories: catlist,
                    search: text,
                    datasource: ESIConfiguration.DataSource,
                    language: null,
                    strict: exactmatch);
                if (list.Character != null)
                {
                    foreach (var i in list.Character)
                    {
                        long id = i ?? -1;
                        if (hasitems.Contains(id))
                            continue;

                        hasitems.Add(id);

                        EntityInfo info = new EntityInfo()
                        {
                            EntityType = EntityType.Character,
                            EntityID = id
                        };

                        newitems.Add(info);

                        if (newitems.Count >= MAX_NAME_LOOKUP)
                        {
                            LookupIDsInternal(newitems.ToArray(), false);
                            ret.AddRange(newitems);
                            newitems.Clear();
                        }
                    }
                }

                if (list.Corporation != null)
                {
                    foreach (var i in list.Corporation)
                    {
                        long id = i ?? -1;
                        if (hasitems.Contains(id))
                            continue;
                        hasitems.Add(id);

                        EntityInfo info = new EntityInfo()
                        {
                            EntityType = EntityType.Corporation,
                            EntityID = id
                        };

                        newitems.Add(info);

                        if (newitems.Count >= MAX_NAME_LOOKUP)
                        {
                            LookupIDsInternal(newitems.ToArray(), false);
                            ret.AddRange(newitems);
                            newitems.Clear();
                        }
                    }
                }

                if (list.Alliance != null)
                {
                    foreach (var i in list.Alliance)
                    {
                        long id = i ?? -1;
                        if (hasitems.Contains(id))
                            continue;
                        hasitems.Add(id);

                        EntityInfo info = new EntityInfo()
                        {
                            EntityType = EntityType.Alliance,
                            EntityID = id
                        };

                        newitems.Add(info);

                        if (newitems.Count >= MAX_NAME_LOOKUP)
                        {
                            LookupIDsInternal(newitems.ToArray(), false);
                            ret.AddRange(newitems);
                            newitems.Clear();
                        }
                    }
                }
                if (newitems.Count > 0)
                {
                    LookupIDsInternal(newitems.ToArray(), false);
                    ret.AddRange(newitems);
                    newitems.Clear();
                }


            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(null, e);
            }
        }
    }
}
