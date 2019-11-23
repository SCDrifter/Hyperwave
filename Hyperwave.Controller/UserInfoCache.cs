using Hyperwave.Common;
using Hyperwave.Config;
using Hyperwave.ViewModel;
using Eve.Api.Api;
using Eve.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperwave.Controller
{
    public class UserInfoCache
    {
        Dictionary<string, EntityData> mCache = new Dictionary<string, EntityData>();
        EveMailClient mClient;

        Task mLastTask = null;

        readonly DateTime EVE_CLIENT_LAUNCH = new DateTime(2003, 4, 6);

        internal UserInfoCache(EveMailClient client)
        {
            mClient = client;
        }

        public bool IsWorking
        {
            get
            {
                return mLastTask != null;
            }
        }

        public EntityData GetData(EntityType type,long id, out bool item_created)
        {
            string name = string.Format("{0}://{1}", type, id);

            EntityData ret;

            item_created = false;

            if (!mCache.TryGetValue(name, out ret))
            {
                ret = CreateItem(type, name);
                item_created = true;
                ret.Subject = new MailRecipient(type, id);
                LoadData(ret,true);
            }

            return ret;
        }

        public EntityData GetData(MailRecipient subject, out bool item_created)
        {
            string name = string.Format("{0}://{1}", subject.Type, subject.Id);

            EntityData ret;

            item_created = false;

            if (!mCache.TryGetValue(name, out ret))
            {
                ret = CreateItem(subject.Type, name);
                item_created = true;
                ret.Subject = subject;
                LoadData(ret, false);
            }

            return ret;
        }

        private void LoadData(EntityData data, bool lookupname)
        {
            if (data == null)
                return;

            mLastTask = LoadDataAsync(data,lookupname);
            mLastTask.ContinueWith(delegate (Task t)
            {
                if (t == mLastTask)
                    mLastTask = null;
            });
        }

        private EntityData CreateItem(EntityType type,string name)
        {
            EntityData ret = null;
            switch(type)
            {
                case EntityType.Character:
                    ret = new CharacterData();
                    break;
                case EntityType.Corporation:
                    ret = new CorporationData();
                    break;
                case EntityType.Alliance:
                    ret = new AllianceData();
                    break;
                default:
                    return null;
            }

            mCache.Add(name, ret);

            return ret;
        }

        private async Task LoadDataAsync(EntityData data, bool lookupname)
        {
            if(lookupname)
            {
                await mClient.AddLookupAsync(data.Subject);
            }
            switch (data.Subject.Type)
            {
                case EntityType.Character:
                    await LoadCharacterData((CharacterData)data);
                    break;
                case EntityType.Corporation:
                    await LoadCorporationData((CorporationData)data);
                    break;
                case EntityType.Alliance:
                    await LoadAllianceData((AllianceData)data);
                    break;
            }

            await mClient.FinishLookupsAsync();
        }

        private async Task LoadAllianceData(AllianceData data)
        {
            AllianceApi api = new AllianceApi();
            

            GetAlliancesAllianceIdOk info;

            try
            {
                int id = (int)data.Subject.Id;
                info = await api.GetAlliancesAllianceIdAsync(
                    allianceId: id, 
                    datasource: ESIConfiguration.DataSource);
            }
            catch(Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(this, e);
                data.LoadFailed = true;
                return;
            }
            data.CreatorCharacter = info.CreatorId.HasValue ? new MailRecipient(EntityType.Character, info.CreatorId.Value) : null;
            data.CreatorCorp = info.CreatorCorporationId.HasValue ? new MailRecipient(EntityType.Corporation, info.CreatorCorporationId.Value) : null;
            data.Executor = info.ExecutorCorporationId.HasValue ? new MailRecipient(EntityType.Corporation, info.ExecutorCorporationId.Value) : null;
            data.Founded = info.DateFounded.HasValue ? info.DateFounded.Value : EVE_CLIENT_LAUNCH;
            data.Ticker = info.Ticker;

            await mClient.AddLookupAsync(data.CreatorCharacter);
            await mClient.AddLookupAsync(data.CreatorCorp);
            await mClient.AddLookupAsync(data.Executor);

            data.InfoLoaded = true;
            data.HistoryLoaded = true;
        }

        private async Task LoadCorporationData(CorporationData data)
        {
            CorporationApi api = new CorporationApi();
            await Task.WhenAll(LoadCorporationInfo(data, api), LoadCorporationHistory(data, api));
        }

        private async Task LoadCorporationInfo(CorporationData data, CorporationApi api)
        {
            GetCorporationsCorporationIdOk info;

            try
            {
                int id = (int)data.Subject.Id;
                info = await api.GetCorporationsCorporationIdAsync(
                    corporationId: id,
                    datasource: ESIConfiguration.DataSource);
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(this, e);
                data.LoadFailed = true;
                return;
            }

            data.Creator = info.CreatorId.HasValue ? new MailRecipient(EntityType.Character, info.CreatorId.Value) : null;
            data.CEO = info.CeoId.HasValue ? new MailRecipient(EntityType.Character, info.CeoId.Value) : null;
            data.Founded = info.DateFounded.HasValue ? info.DateFounded.Value : EVE_CLIENT_LAUNCH;
            data.Ticker = info.Ticker;
            
            data.PrimaryMembership = info.AllianceId.HasValue ? new MailRecipient(EntityType.Alliance, info.AllianceId.Value) : null;

            data.Description = info.Description;
            data.TaxRate = info.TaxRate.HasValue ? info.TaxRate.Value : 0;
            data.Url = info.Url;

            await mClient.AddLookupAsync(data.PrimaryMembership);
            await mClient.AddLookupAsync(data.Creator);
            await mClient.AddLookupAsync(data.CEO);

            data.HasSecurityStatus = false;

            data.InfoLoaded = true;
        }

        private async Task LoadCorporationHistory(CorporationData data, CorporationApi api)
        {
            List<GetCorporationsCorporationIdAlliancehistory200Ok> items;

            try
            {
                int id = (int)data.Subject.Id;
                items = await api.GetCorporationsCorporationIdAlliancehistoryAsync(
                    corporationId: id, 
                    datasource: ESIConfiguration.DataSource);
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(this, e);
                return;
            }
            HistoryItem[] history = new HistoryItem[items.Count];
            HistoryItem last_item = null;

            for (int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                var hitem = history[i] = new HistoryItem();

                hitem.Organization = new MailRecipient(EntityType.Alliance, item.AllianceId.HasValue ? item.AllianceId.Value : 0);

                await mClient.AddLookupAsync(hitem.Organization);

                hitem.Closed = item.IsDeleted.HasValue && item.IsDeleted.Value;
                hitem.HasStartDate = item.StartDate.HasValue;
                if (hitem.HasStartDate)
                    hitem.StartDate = item.StartDate.Value;
                else
                    hitem.StartDateText = "(Unknown)";

                hitem.HasEndDate = false;
                hitem.EndDateText = "(Present)";

                if (last_item != null)
                {
                    last_item.HasEndDate = hitem.HasStartDate;
                    last_item.EndDate = hitem.StartDate;
                    last_item.EndDateText = hitem.StartDateText;
                }

                last_item = hitem;
            }

            data.History = history;
            data.HistoryLoaded = true;
        }

        private async Task LoadCharacterData(CharacterData data)
        {
            CharacterApi api = new CharacterApi();
            await Task.WhenAll(LoadCharacterInfo(data, api), LoadCharacterHistory(data, api));
        }

        Dictionary<int, string> mBloodlines = new Dictionary<int, string>();
        Dictionary<int, string> mRaces = new Dictionary<int, string>();

        private async Task LoadRacesAndBloodlines()
        {
            UniverseApi api = new UniverseApi();
            
            if (mBloodlines.Count == 0)
            {
                List<GetUniverseBloodlines200Ok> bloodlines = await api.GetUniverseBloodlinesAsync(
                    datasource: ESIConfiguration.DataSource);

                foreach(var i in bloodlines)
                {
                    mBloodlines.Add(i.BloodlineId.Value, i.Name);
                }
            }

            if (mRaces.Count == 0)
            {
                List<GetUniverseRaces200Ok> races = await api.GetUniverseRacesAsync(
                    datasource: ESIConfiguration.DataSource);

                foreach (var i in races)
                {
                    mRaces.Add(i.RaceId.Value, i.Name);
                }
            }
        }

        private async Task LoadCharacterInfo(CharacterData data, CharacterApi api)
        {
            GetCharactersCharacterIdOk info;

            try
            {
                await LoadRacesAndBloodlines();

                int id = (int)data.Subject.Id;
                info = await api.GetCharactersCharacterIdAsync(
                    characterId: id, 
                    datasource: ESIConfiguration.DataSource);
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(this, e);
                data.LoadFailed = true;
                return;
            }

            data.PrimaryMembership = info.CorporationId.HasValue ? new MailRecipient(EntityType.Corporation, info.CorporationId.Value) : null;
            data.SecondaryMembership = info.AllianceId.HasValue ? new MailRecipient(EntityType.Alliance, info.AllianceId.Value) : null;

            data.Description = info.Description;

            data.Race = mRaces.GetValueOrDefault(info.RaceId.GetValueOrDefault(-1), "(Unknown)");
            data.Bloodline = mBloodlines.GetValueOrDefault(info.BloodlineId.GetValueOrDefault(-1), "(Unknown)");

            data.Birthday = info.Birthday.GetValueOrDefault(EVE_CLIENT_LAUNCH);
            data.Gender = info.Gender.GetValueOrDefault().ToString();
            data.SecurityStatus = info.SecurityStatus.GetValueOrDefault();
            data.HasSecurityStatus = true;
                        
            await mClient.AddLookupAsync(data.PrimaryMembership);
            await mClient.AddLookupAsync(data.SecondaryMembership);
                        
            data.InfoLoaded = true;
        }

        private async Task LoadCharacterHistory(CharacterData data, CharacterApi api)
        {
            List<GetCharactersCharacterIdCorporationhistory200Ok> items;

            try
            {
                int id = (int)data.Subject.Id;
                items = await api.GetCharactersCharacterIdCorporationhistoryAsync(
                    characterId: id, 
                    datasource: ESIConfiguration.DataSource);
            }
            catch (Eve.Api.Client.ApiException e)
            {
                ExceptionHandler.HandleApiException(this, e);
                return;
            }
            HistoryItem[] history = new HistoryItem[items.Count];
            HistoryItem last_item = null;

            for (int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                var hitem = history[i] = new HistoryItem();

                hitem.Organization = new MailRecipient(EntityType.Corporation, item.CorporationId.GetValueOrDefault(-1));

                await mClient.AddLookupAsync(hitem.Organization);

                hitem.Closed = item.IsDeleted.HasValue && item.IsDeleted.Value;
                hitem.HasStartDate = item.StartDate.HasValue;
                if (hitem.HasStartDate)
                    hitem.StartDate = item.StartDate.Value;
                else
                    hitem.StartDateText = "(Unknown)";

                hitem.HasEndDate = false;
                hitem.EndDateText = "(Present)";

                if (last_item != null)
                {
                    last_item.HasEndDate = hitem.HasStartDate;
                    last_item.EndDate = hitem.StartDate;
                    last_item.EndDateText = hitem.StartDateText;
                }

                last_item = hitem;
            }

            data.History = history;
            data.HistoryLoaded = true;
        }
    }
}
